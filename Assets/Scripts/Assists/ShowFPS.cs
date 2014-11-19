using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour {

	private float updateInterval = 0.5f;

	private double lastInterval;
	private int frames;
	private float currFPS;

	void Start () 
	{
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}

	void Update () 
	{
		++frames;

		float timeNow = Time.realtimeSinceStartup;

		if (timeNow > lastInterval + updateInterval)
		{
			currFPS = (float)(frames / (timeNow - lastInterval));
			lastInterval = timeNow;
			frames = 0;
		}
	}

	private void OnGUI()
	{
		GUILayout.Label("  FPS: " + currFPS.ToString("f2"));
	}
}
