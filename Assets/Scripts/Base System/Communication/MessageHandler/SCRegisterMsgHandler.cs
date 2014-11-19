using UnityEngine;
using Communication;
using Hero.Message.Auto;

public class SCRegisterMsgHandler : ICommunicationMassagerHandler
{
	public int GetMessageID()
	{
		return MessageTypeConstants.SC_REGISTER;
	}
	
	public void HandleMessage(object message)
	{
		Debug.Log(ToString() + " -> " + message);
	}
}