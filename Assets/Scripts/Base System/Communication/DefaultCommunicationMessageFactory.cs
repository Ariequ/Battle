using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Communication;
using Thrift.Protocol;
using Hero.Message;
using Hero.Message.Auto;

internal class DefaultCommunicationMessageFactory : ICommunicationMessageFactory
{
	private Dictionary<int, Type> messateIDMapping = new Dictionary<int, Type>();
	private Dictionary<Type, int> messateTypeMapping = new Dictionary<Type, int>();

	private int orderIDIncreaser = 1;

	private string sessionKey = null;

	private bool encrypt;

	private bool compression;

	public DefaultCommunicationMessageFactory (bool encrypt = false, bool compression = false)
	{
		this.encrypt = encrypt;
		this.compression = compression;

		messateIDMapping.Add (MessageTypeConstants.CS_REGISTER, typeof(CSRegisterMsg));
		messateIDMapping.Add (MessageTypeConstants.CS_LOGIN, typeof(CSLoginMsg));
		messateIDMapping.Add (MessageTypeConstants.CS_CREATE_NEW_CHAR, typeof(CSCreateNewCharMsg));
		messateIDMapping.Add (MessageTypeConstants.CS_QUERY_CHARACTER_INFO, typeof(CSCharacterInfoMsg));
		messateIDMapping.Add (MessageTypeConstants.CS_RANDOM_NAME, typeof(CSRandomNameMsg));
		
		messateIDMapping.Add (MessageTypeConstants.SC_REGISTER, typeof(SCRegisterMsg));
		messateIDMapping.Add (MessageTypeConstants.SC_LOGIN, typeof(SCLoginMsg));
		messateIDMapping.Add (MessageTypeConstants.SC_NOTICE_CREATE_NEW_CHAR, typeof(SCNoticeCreateNewCharMsg));
		messateIDMapping.Add (MessageTypeConstants.SC_CREATE_NEW_CHAR, typeof(SCCreateNewCharMsg));
		messateIDMapping.Add (MessageTypeConstants.SC_CHARACTER_INFO, typeof(SCCharacterInfoMsg));
		messateIDMapping.Add (MessageTypeConstants.SC_RANDOM_NAME, typeof(SCRandomNameMsg));
		messateIDMapping.Add (MessageTypeConstants.SC_SYSTEM_INFO, typeof(SCSystemInfoMsg));
		
		foreach (KeyValuePair<int, Type> kvPaire in messateIDMapping)
		{
			messateTypeMapping.Add(kvPaire.Value, kvPaire.Key);
		}
	}

	public IRequestPacket CreateCommunicationMessage (object message)
	{
		TBase thriftObject = message as TBase;

		if (thriftObject != null)
		{
			int messageID = getMessageType (thriftObject);

			Header header = new Header();
			header.OrderId = orderIDIncreaser ++;
			header.Sk = sessionKey;
			
			byte[] headerBytes = ThriftSerialize.Serialize (header);
			byte[] bodyBytes = ThriftSerialize.Serialize (thriftObject);
			
			int currentIndex = 0;
			byte[] messageBytes = ByteArrayUtil.intToBytes (messageID);
			byte[] sendba = new byte[ messageBytes.Length + 4 + 2 + headerBytes.Length + 4 + bodyBytes.Length ];
			
			messageBytes.CopyTo (sendba, currentIndex);
			currentIndex += messageBytes.Length;
			
			sendba [currentIndex++] = (byte) (this.encrypt ? 1 : 0);
			sendba [currentIndex++] = (byte) (this.compression ? 1 : 0);
			sendba [currentIndex++] = 0;
			sendba [currentIndex++] = 0;
			
			byte[] headerLength = ByteArrayUtil.shortToByteArray ((short)headerBytes.Length);
			headerLength.CopyTo (sendba, currentIndex);
			currentIndex += headerLength.Length;
			headerBytes.CopyTo (sendba, currentIndex);
			currentIndex += headerBytes.Length;

			byte[] bodyLength = ByteArrayUtil.intToBytes (bodyBytes.Length);
			bodyLength.CopyTo (sendba, currentIndex);
			currentIndex += bodyLength.Length;
			bodyBytes.CopyTo (sendba, currentIndex);

			return new DefaultRequestPacket(messageID, header.OrderId, sendba);
		}
		else
		{
			Debug.LogError("Communication Message must be extended from TBase.");
		}

		return null;
	}
	
	public IResponsePacket CreateCommunicationMessage (byte[] message)
	{
		int pointer = 0;

		byte[] buff = ReadFromByteArray (message, MessageConfig.MSG_ID_LEN, ref pointer);
		Array.Reverse (buff);
		int messageID = BitConverter.ToInt32 (buff, 0);

		Debug.Log ("Message ID: " + messageID);

		bool encrypt = message [pointer++] != 0;
		bool compression = message[pointer++] != 0;
		pointer += 2;

		buff = ReadFromByteArray (message, MessageConfig.MSG_HEADER_LEN, ref pointer);
		Array.Reverse (buff);
		short headerLen = BitConverter.ToInt16 (buff, 0);

		Header header = new Header();

		if (headerLen > 0)
		{
			buff = ReadFromByteArray (message, headerLen, ref pointer);
			ThriftSerialize.DeSerialize(header, buff);
		}

		buff = ReadFromByteArray (message, MessageConfig.MSG_BODY_LEN, ref pointer);
		Array.Reverse (buff);
		int bodyLen = BitConverter.ToInt32 (buff, 0);

		buff = ReadFromByteArray (message, bodyLen, ref pointer);
		TBase thriftObject = getMessage (messageID);
		ThriftSerialize.DeSerialize (thriftObject, buff);

		if (header.__isset.sk)
		{
			this.sessionKey = header.Sk;
		}

		Debug.Log ("Order Id: " + header.OrderId);
		Debug.Log ("Session Key: " + header.Sk);
		Debug.Log ("Message Body: " + thriftObject);

		return new DefaultResponsePacket (messageID, header.OrderId, thriftObject);
	}

	private TBase getMessage (int messageId)
	{
		Type type = messateIDMapping[messageId];
		
		return (TBase) Activator.CreateInstance(type);
	}
	
	private int getMessageType (TBase message)
	{
		Type type = message.GetType ();
		
		return messateTypeMapping[type];
	}

	private static byte[] ReadFromByteArray (byte[] byteArray, int readSize, ref int offset)
	{
		byte[] buffer = new byte[readSize];
		Array.Copy (byteArray, offset, buffer, 0, buffer.Length);
		offset += buffer.Length;

		return buffer;
	}
}