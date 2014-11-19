using UnityEngine;
using System.Collections;

public class ButtonOnClick: MonoBehaviour {

	public static GameObject buttonobj;

	public void onClick(GameObject obj)
	{
		buttonobj = obj;
		Debug.Log (obj.name);
	}
	
}
