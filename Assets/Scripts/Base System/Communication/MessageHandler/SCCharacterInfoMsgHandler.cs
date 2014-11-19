using UnityEngine;
using Communication;
using Hero.Message.Auto;

public class SCCharacterInfoMsgHandler : ICommunicationMassagerHandler
{
	public int GetMessageID()
	{
		return MessageTypeConstants.SC_CHARACTER_INFO;
	}
	
	public void HandleMessage(object message)
	{
		Debug.Log(ToString() + " -> " + message);
	}
}