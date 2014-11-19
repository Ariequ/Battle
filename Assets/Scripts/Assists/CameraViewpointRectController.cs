using UnityEngine;
using System.Collections;

public class CameraViewpointRectController : MonoBehaviour 
{
	void Awake () 
	{
		float width = Screen.width;
		float height = width * (9f / 16f);
		Camera.main.pixelRect = new Rect (0, (Screen.height - height) / 2f, width, height);
	}
}
