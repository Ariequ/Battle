using Communication;
using Thrift.Protocol;
using UnityEngine;

public class CommunicationCenter
{
	private const string SERVER_ADDRESS = "27.131.223.98";
//	private const string SERVER_ADDRESS = "192.168.10.110";

	private CommunicationManager communictionManager;

    public CommunicationCenter()
    {
		communictionManager = new CommunicationManager(new SocketClient(SERVER_ADDRESS,"8090"), new DefaultCommunicationMessageFactory());

		communictionManager.RegistMessageHandler (new SCNoticeCreateNewCharMsgHandler());
		communictionManager.RegistMessageHandler (new SCSystemInfoMsgHandler());
		communictionManager.RegistMessageHandler (new SCCharacterInfoMsgHandler()); 
    }

    public void SendMessage(TBase message, CommunicationEventHandler callback)
    {
		communictionManager.SendMessage(message, callback);
    }

    public void Update()
    {
        communictionManager.Update();
    }
}

