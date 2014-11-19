using UnityEngine;
using Communication;
using Hero.Message.Auto;

public class SCSystemInfoMsgHandler : AbstractCommunicationMessageHandler {
	
	public override int GetMessageID()
	{
		return MessageTypeConstants.SC_SYSTEM_INFO;
	}
	
	public override void HandleMessage(object message)
	{
		Debug.Log(ToString() + " -> " + message);
	}
}
