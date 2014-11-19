using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitializer : MonoBehaviour 
{
	void Awake () 
	{
		Application.targetFrameRate = 60;
		MetaManager.InitializeSingleton (new MockData());
		MockServer.Initialize ();
		ApplicationFacade.Instance.StartUp ();

//		StaticConfigManager.Instance.LoadConfig ();
	}
}
