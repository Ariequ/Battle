﻿using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	public float waitSeconds = 1.0f;

	void Start()
	{
		Destroy(this.gameObject, this.waitSeconds / BattleTime.timeScale);
	}
}
