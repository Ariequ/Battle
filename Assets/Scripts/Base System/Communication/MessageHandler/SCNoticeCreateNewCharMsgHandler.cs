using UnityEngine;
using Communication;
using Hero.Message.Auto;

public class SCNoticeCreateNewCharMsgHandler : AbstractCommunicationMessageHandler {

	public override int GetMessageID()
	{
		return MessageTypeConstants.SC_NOTICE_CREATE_NEW_CHAR;
	}
	
	public override void HandleMessage(object message)
	{
		Debug.Log(ToString() + " -> " + message);

		SendMessage (new CSRandomNameMsg(), HandlerRandomNameMessage);
	}

	private void HandlerRandomNameMessage (object message)
	{
		Debug.Log(ToString() + " -> " + message);

		SCRandomNameMsg randomNameMsg = (SCRandomNameMsg) message;
		CSCreateNewCharMsg createNewCharMsg = new CSCreateNewCharMsg ();
		createNewCharMsg.CharName = randomNameMsg.Name;
 
		SendMessage (createNewCharMsg, HandlerCreateNewCharacterMessage);
	}

	private void HandlerCreateNewCharacterMessage (object message)
	{
		Debug.Log(ToString() + " -> " + message);
	}
}
