using UnityEngine;
using System.Collections;

[AddComponentMenu("Game/Time Controller")]
public class TimeController : MonoBehaviour 
{
	[Range(0.1f, 1000.0f)]
	public float timeScale = 1.0f;

	void Awake ()
	{
		BattleTime.timeScale = timeScale;
	}

	void Update () 
	{
		BattleTime.time = Time.time * BattleTime.timeScale;
		BattleTime.deltaTime = Time.deltaTime * BattleTime.timeScale;
	}
}
