using System;
using UnityEngine;

public class ExplosionInfo : MonoBehaviour
{
	public bool alwaysFlyingSoldier = true;

	public float radius = 3f;

	public float maxForce = 700f;

	public float minForce = 500f;

	public float maxTorque = 50f;

	public float minTorque = 20f;

	public int maxFlyingNum = 5;

	public int minFlyingNum = 2;

	//Calculate value after a short delay to make the effects more realistic.
	public float valueDelay = 0f;
}
