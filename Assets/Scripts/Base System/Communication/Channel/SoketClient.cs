#define	USE_THREAD_RECEIVE_MSG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using UnityEngine;
using Thrift.Protocol;
using Hero.Message.Auto;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Communication
{
	class ByteBuffer
	{
		private MemoryStream leftRecvMsgBuf;
		
		public ByteBuffer (int maxSize)
		{
			this.leftRecvMsgBuf = new MemoryStream (maxSize);
		}
		
		public void Put (byte[] data, int length)
		{
			leftRecvMsgBuf.Write (data, 0, length);
		}
		
		public void Put (byte[] data)
		{
			leftRecvMsgBuf.Write (data, 0, data.Length);
		}
		
		public void Get (byte[] data)
		{
			leftRecvMsgBuf.Read (data, 0, data.Length);
		}
		
		public void Flip ()
		{
			this.leftRecvMsgBuf.Seek (0, SeekOrigin.Begin);
		}
		
		public void Compact ()
		{
			if (leftRecvMsgBuf.Position == 0) {
				return;
			}
			
			long _remaining = this.Remaining ();
			if (_remaining <= 0) {
				this.Clear ();
				return;
			}
			
			byte[] _leftData = new byte[_remaining];
			this.Get (_leftData);
			this.Clear ();
			this.Put (_leftData);            
		}
		
		public void Clear ()
		{
			leftRecvMsgBuf.Seek (0, SeekOrigin.Begin);
			leftRecvMsgBuf.SetLength (0);
		}
		
		public long Remaining ()
		{
			return leftRecvMsgBuf.Length - leftRecvMsgBuf.Position;
		}
		
		public Boolean HasRemaining ()
		{
			return leftRecvMsgBuf.Length > leftRecvMsgBuf.Position;
		}
		
		public long Position ()
		{
			return leftRecvMsgBuf.Position;
		}
		
		public long Length ()
		{
			return leftRecvMsgBuf.Length;
		}
		
		public void SetPosition (long position)
		{
			leftRecvMsgBuf.Seek (position, SeekOrigin.Begin);
		}

		public void Jump (long offset)
		{
			leftRecvMsgBuf.Seek (leftRecvMsgBuf.Position + offset, SeekOrigin.Begin);
		}
	}
	
	public enum EClientConnectState
	{
		CONNECT_STATE_NONE,
		CONNECT_STATE_TIME_OUT,
		
		CONNECT_STATE_CAN_RECONNECT,
		
		CONNECT_STATE_TRY_CONNECT,
		CONNECT_STATE_CONNECTED,
		CONNECT_STATE_DO_TRY_CONNECT
	}
	
/**
     *
     */
	struct IPaddressWrapper
	{
		public IPEndPoint ipPoint;
		public bool isTried;
	};
	
/**
     *
     */
	class MsgReceiveHelper
	{
		public Socket socket;
		public byte[] buffer;
	}
	
/**
     *  socket Client
     */
	public class SocketClient : IChannel
	{
		private const int MAX_MSG_PER_LOOP = 16;
		EClientConnectState ConnectState = EClientConnectState.CONNECT_STATE_NONE;
		
		/** */
		IPaddressWrapper[] ipAddressArry;
		/** socket Client */
		Socket socketClient;
		/**  */
		IList<byte[]> recMsgs;
		IList<byte[]> msgCopy;
		ByteBuffer recMsgBuf;
		/**  */
		const int DEFAULT_RECEIVE_SIZE = 64 * 1024;
		const int DEFAULT_SEND_SIZE = 32 * 1024;

		//
		#if USE_THREAD_RECEIVE_MSG
		Thread	mRecvThread = null;
		bool	mThreadWork = false;
		#endif
		//
		bool	m_bSecurityPolicy = false;
		//
		static	private	object		_ErrorLock = new object ();
		static	private	int			_ShowErrorIndex = 0;
		static	private	int			_ErrorCode = 0;
		static	private	SocketError	_SocketError;
		
		public SocketClient (String serverIp, String serverPorts)
		{
			socketClient = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			// Set to no blocking
			socketClient.Blocking = false;
			socketClient.ReceiveBufferSize = DEFAULT_RECEIVE_SIZE;
			socketClient.SendBufferSize = DEFAULT_SEND_SIZE;
			socketClient.ReceiveTimeout = 30000;
			socketClient.SendTimeout = 30000;
			recMsgs = new List<byte[]> ();
			msgCopy = new List<byte[]> ();

			this.InitIpAddressArry (serverIp, serverPorts);
			
			recMsgBuf = new ByteBuffer (MessageConfig.MAX_MSG_LEN);
			
			// Unity Network Security
			m_bSecurityPolicy = false;
			if (Application.isWebPlayer) {
				m_bSecurityPolicy = true;
			} else if (RuntimePlatform.WindowsEditor == Application.platform || RuntimePlatform.OSXEditor == Application.platform) {
				#if UNITY_EDITOR
				if( BuildTarget.WebPlayer == EditorUserBuildSettings.activeBuildTarget || BuildTarget.NaCl == EditorUserBuildSettings.activeBuildTarget )
				{
					m_bSecurityPolicy = true;
				}
				#endif
			}
		}
		
		public void Close ()
		{
			if (socketClient == null) {
				return;
			}
			
			#if USE_THREAD_RECEIVE_MSG
			mThreadWork = false;
			#endif
			
			socketClient.Close ();
		}

		//
		public void DoRetryConnect ()
		{
			ConnectState = EClientConnectState.CONNECT_STATE_DO_TRY_CONNECT;
		}
		
		/**
         *
         */
		private IPEndPoint GetServerAddress ()
		{
			for (int i = 0; i < ipAddressArry.Length; i++) {
				IPaddressWrapper _wrapper = ipAddressArry [i];
				//
				if (_wrapper.isTried) {
					continue;
				}
				_wrapper.isTried = true;
				// FIXME why
				ipAddressArry [i] = _wrapper;
				Console.WriteLine ("Try to connect : " + _wrapper.ipPoint);
				return _wrapper.ipPoint;
			}
			
			return null;
		}
		
		/**
         *
         */
		public void ResetServerAddressStatus ()
		{
			ConnectState = EClientConnectState.CONNECT_STATE_NONE;
			for (int i = 0; i < ipAddressArry.Length; i++) {
				IPaddressWrapper _wrapper = ipAddressArry [i];
				_wrapper.isTried = false;
				ipAddressArry [i] = _wrapper;
			}
		}
		
		/**
         * 
         */
		private void InitIpAddressArry (String serverIp, String ports)
		{
			IPAddress _ipAddress = IPAddress.Parse (serverIp);
			string[] _tempArray = ports.Split (',');
			int _portSize = _tempArray.Length;
			ipAddressArry = new IPaddressWrapper[_portSize];
			for (int i = 0; i < _portSize; i++) {
				int _port = Convert.ToInt32 (_tempArray [i].Trim ());
				IPaddressWrapper _addressWrapper = new IPaddressWrapper ();
				_addressWrapper.ipPoint = new IPEndPoint (_ipAddress, _port);
				_addressWrapper.isTried = false;
				ipAddressArry [i] = _addressWrapper;
			}
		}


		/// <summary>
		/// Tries the connect.
		/// </summary>
		/// <exception cref='Exception'>
		/// Represents errors that occur during application execution.
		/// </exception>
		public void TryConnect ()
		{
			try {
				IPEndPoint _ep = null;
				
				//
				if (m_bSecurityPolicy) {

				} else {
					_ep = this.GetServerAddress ();
					if (null == _ep) {
						ConnectState = EClientConnectState.CONNECT_STATE_TIME_OUT;
						ClientLog.LogError ("Connect timeout : no valid server ip or port");
						return;
					}
				}

				ConnectState = EClientConnectState.CONNECT_STATE_TRY_CONNECT;
				socketClient.BeginConnect (_ep, new AsyncCallback (ConnectCallback), socketClient);
				ClientLog.Log ("Connect server : " + _ep.Address.ToString () + ":" + _ep.Port.ToString ());
			} catch (Exception ex) {
				ClientLog.LogError (ex.ToString ());
			}
		}

		
		/**
         *
         */
		private void ConnectCallback (IAsyncResult ar)
		{
			try {
				Socket _socket = (Socket)ar.AsyncState;
				if (!_socket.Connected) {
					ClientLog.LogError (_socket.LocalEndPoint + " connect failed!, try connect again");
					this.DoRetryConnect ();
				} else {
					ConnectState = EClientConnectState.CONNECT_STATE_CONNECTED;
					_socket.EndConnect (ar);
					ClientLog.Log (_socket.LocalEndPoint + " connect successful!");
					this.StartRecevieMsg ();
				}
			} catch (Exception e) {
				
				ConnectState = EClientConnectState.CONNECT_STATE_TIME_OUT;
				// do something
				Console.WriteLine (e.ToString ());
			}
			
		}
		
		/**
         *
         */
		public bool IsConnected ()
		{
			return (null != this.socketClient && this.socketClient.Connected);
		}
		
		// login ui can retry ?
		public bool CanTryConnect ()
		{
			return (!IsConnected () && ConnectState < EClientConnectState.CONNECT_STATE_CAN_RECONNECT);
		}
		
		/**
         *
         */
		public void SendMessage (byte[] bytes)
		{
			try {
				if (!this.IsConnected ()) {
					ClientLog.LogError ("server is not connected!");
				} else {
					socketClient.BeginSend (bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback (SendMsgCallback), socketClient);
				}
			} catch (Exception ex) {
				ClientLog.LogError (ex.ToString ());
			}
		}
		
		//
		private void StartRecevieMsg ()
		{			
			#if USE_THREAD_RECEIVE_MSG
			if (null == mRecvThread)
				mRecvThread = new Thread (RecvThreadDoWork);
			//
			if (null != mRecvThread) {
				mThreadWork = true;
				mRecvThread.Start ();
			}
			#else
			MsgReceiveHelper _receiveHelper = new MsgReceiveHelper ();
			_receiveHelper.socket = this.socketClient;
			_receiveHelper.buffer = new byte[DEFAULT_RECEIVE_SIZE];
			
			this.socketClient.BeginReceive (_receiveHelper.buffer, 0, _receiveHelper.buffer.Length, SocketFlags.None, new AsyncCallback (ReceiveMsgCallback), _receiveHelper);
			#endif
		}
		
		//
		#if USE_THREAD_RECEIVE_MSG
		void	RecvThreadDoWork ()
		{
			byte[] _buffer = new byte[DEFAULT_RECEIVE_SIZE];
			while (mThreadWork) {
				try {
					int _recSize = this.socketClient.Receive (_buffer, DEFAULT_RECEIVE_SIZE, SocketFlags.None);
					if (_recSize > 0) {
						this.DecodeMsg (_buffer, _recSize);
					}
					// < 0, the remote socket is close...
					else {
						SetShowNetworkError (1, 0, SocketError.SocketError);					
						ClientLog.LogError ("Socket EndReceive failed, the size is 0. The remote socket is closed. Disconnect...");
						this.Close ();
						break;
					}
				} catch (SocketException se) {
					if (se.SocketErrorCode == SocketError.WouldBlock ||
						se.SocketErrorCode == SocketError.IOPending ||
						se.SocketErrorCode == SocketError.NoBufferSpaceAvailable) {
						// socket buffer is probably empty, wait and try again
						Thread.Sleep (50);
					} else {
						SetShowNetworkError (2, se.ErrorCode, se.SocketErrorCode);							
						ClientLog.LogError ("receive msg failed : " + se.ToString ());
						ClientLog.LogError ("Socket EndReceive Exception, ErrorCode = " + se.ErrorCode.ToString () + ", SocketErrorCode = " + se.SocketErrorCode.ToString ());
						ClientLog.LogError ("Socket fatal exception, disconnect...");
						this.Close ();
						break;
					}
				}
				
				Thread.Sleep (1);
			}
			
			//
			mRecvThread.Join ();
		}
		#endif
		
		/**
         *
         */
		private void ReceiveMsgCallback (IAsyncResult receiveRes)
		{
			if (!IsConnected ()) {
				ClientLog.LogError ("ReceiveMsgCallback : the socket is not connected!!!");
				return;
			}
			
			MsgReceiveHelper _receiveHelper = null;
			try {
				int _recSize = this.socketClient.EndReceive (receiveRes);
				if (_recSize > 0) {
					_receiveHelper = (MsgReceiveHelper)receiveRes.AsyncState;
					this.DecodeMsg (_receiveHelper.buffer, _recSize);
				}
				// < 0, the remote socket is close...
				else {
					SetShowNetworkError (1, 0, SocketError.SocketError);					
					ClientLog.LogError ("Socket EndReceive failed, the size is 0. The remote socket is closed. Disconnect...");
					this.Close ();
					return;
				}
			}
			//
			catch (SocketException se) {
				SetShowNetworkError (2, se.ErrorCode, se.SocketErrorCode);				
				ClientLog.LogError ("receive msg failed : " + se.ToString ());
				ClientLog.LogError ("Socket EndReceive Exception, ErrorCode = " + se.ErrorCode.ToString () + ", SocketErrorCode = " + se.SocketErrorCode.ToString ());
				
				// Disconnect, WSAEWOULDBLOCK
				if (!se.SocketErrorCode.Equals (SocketError.WouldBlock)) {					
					ClientLog.LogError ("Socket fatal exception, disconnect...");
					this.Close ();
					return;
				}
			} catch (Exception e) {
				SetShowNetworkError (3, 0, SocketError.SocketError);				
				ClientLog.LogError ("receive msg failed : " + e.ToString ());
			}
			//
			finally {
				if (_receiveHelper != null) {
					_receiveHelper.socket.BeginReceive (_receiveHelper.buffer, 0, _receiveHelper.buffer.Length, SocketFlags.None, new AsyncCallback (ReceiveMsgCallback), _receiveHelper);
				} else {
					this.StartRecevieMsg ();
				}
			}
		}
		
		/**
         *
         */
		private void SendMsgCallback (IAsyncResult sendRes)
		{
			try {
				Socket _socket = (Socket)sendRes.AsyncState;
				int nSentByte = _socket.EndSend (sendRes);
				
				if (nSentByte <= 0) {
					SetShowNetworkError (4, 0, SocketError.SocketError);				
					ClientLog.LogError ("send msg failed!");
				}
			} catch (Exception e) {
				ClientLog.LogError ("send msg failed : " + e.ToString ());
				//
				SocketException se = e as SocketException;
				if (null != se) {
					SetShowNetworkError (5, se.ErrorCode, se.SocketErrorCode);					
					ClientLog.LogError ("Socket EndSend Exception, ErrorCode = " + se.ErrorCode.ToString () + ", SocketErrorCode = " + se.SocketErrorCode.ToString ());
				}
			}
		}
		
		private byte[] encrytData (byte[] sendData)
		{
			return sendData;
		}
		
		private byte[] decrytData (byte[] receiveData)
		{
			return receiveData;
		}

		private void DecodeMsg (byte[] receiveBytes, int size)
		{
			recMsgBuf.SetPosition (recMsgBuf.Length ());
			recMsgBuf.Put (receiveBytes, size);
			recMsgBuf.Flip ();

			int minCheckLength = MessageConfig.HEADER_LEN_OFFSET + MessageConfig.MSG_HEADER_LEN + MessageConfig.MSG_BODY_LEN;

			while (recMsgBuf.Remaining() >= minCheckLength)
			{
				recMsgBuf.SetPosition(MessageConfig.HEADER_LEN_OFFSET);

				byte[] _buff = new byte[MessageConfig.MSG_HEADER_LEN];
				recMsgBuf.Get (_buff);
				Array.Reverse (_buff);
				
				short headerLen = BitConverter.ToInt16 (_buff, 0);

				if (recMsgBuf.Remaining() >= headerLen + MessageConfig.MSG_BODY_LEN)
				{
					recMsgBuf.Jump(headerLen);

					_buff = new byte[MessageConfig.MSG_BODY_LEN];
					recMsgBuf.Get (_buff);
					Array.Reverse (_buff);

					int bodyLen = BitConverter.ToInt32 (_buff, 0);

					if (recMsgBuf.Remaining() >= bodyLen)
					{
						recMsgBuf.Flip ();

						byte[] data = new byte[minCheckLength + headerLen + bodyLen];
						recMsgBuf.Get(data);
						recMsgs.Add (data);	

						recMsgBuf.Compact ();
						recMsgBuf.Flip();
					}
					else
					{
						break ;
					}
				}
				else
				{
					break ;
				}
			}
		}
        
		//		//
		public void HandleReceiveMsgs ()
		{
			lock (this.recMsgs) {
				int iMsgCount = Math.Min (this.recMsgs.Count, MAX_MSG_PER_LOOP);
				for (int iLoop = 0; iLoop < iMsgCount; ++iLoop) {
					msgCopy.Add (recMsgs [0]);
					recMsgs.RemoveAt (0);
				}
			}
		}
		
		//
		public int GetHandleMsgCount ()
		{
			return msgCopy.Count;
		}
		
		//
		public byte[] PopHandleMsg ()
		{
			if (msgCopy.Count > 0) 
			{
				byte[] Msg = msgCopy [0];
				msgCopy.RemoveAt (0);
				
				return Msg;
			}
			
			return null;
		}  
		
		//
		public EClientConnectState ClientConnectState {
			get{ return ConnectState; }
			set {
				ConnectState = value;
			}
		}

		static	private	void SetShowNetworkError (int iIndex, int iError, SocketError socketErr)
		{
			lock (_ErrorLock) {
				_ShowErrorIndex = iIndex;
				_ErrorCode = iError;
				_SocketError = socketErr;
			}
		}
	}
}

