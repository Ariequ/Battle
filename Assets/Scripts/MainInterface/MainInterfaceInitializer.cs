using UnityEngine;
using System.Collections;

using PureMVC.Patterns;

using Communication;
using Hero.Message.Auto;

public class MainInterfaceInitializer : MonoBehaviour {

	private CommunicationCenter communication;

	// Use this for initialization
	void Start ()
	{
		ApplicationFacade facade = ApplicationFacade.Instance;
		
		facade.RegisterMediator(new MainInterfaceMediator());
		
		facade.SendNotification(NotificationConst.MAIN_INTERFACE_SHOW);

//		communication = new CommunicationCenter();
//
//		UserPasswordLogin userpassword = new UserPasswordLogin ();
//		userpassword.Username = "robot10001";
//		userpassword.Password = "1";
//		
//	 	CSLoginMsg loginMsg = new CSLoginMsg();
//		loginMsg.ChannelCode = ChannelCodeConstants.LOCAL_USER_PWD;
//		loginMsg.UserpasswordLogin = userpassword;
//		loginMsg.DeviceId = "mh170";
//		loginMsg.DeviceModel = "BoYin";
//		loginMsg.Terminal = Terminal.MOBIE;
//		loginMsg.OsType = OSType.Android;
//		loginMsg.OsVersion = "4.0.1";
//		
//		communication.SendMessage(loginMsg, new CommunicationEventHandler(mySuccess));
//	}
//
//	void Update () 
//	{
//		communication.Update();
	}

	void OnDestroy ()
	{
		ApplicationFacade.Instance.RemoveMediator (MainInterfaceMediator.NAME);
	}
	
	public void mySuccess(object message)
	{
		Debug.Log("Fall Down !");
	}
}
