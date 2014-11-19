using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadiusCalculator : MonoBehaviour
{
//	public GameObject sphere;
	
	public ParticleEmitter mainEmitter;
	
	public float radius { get; private set; }
	
	void Awake () 
	{
		if (mainEmitter)
		{
			radius = mainEmitter.minSize;
		}
		else
		{
			float maxRadius = 0;
			
			ParticleEmitter[] emitters = GetComponentsInChildren<ParticleEmitter>();
			
			foreach (ParticleEmitter emitter in emitters)
			{
				if (emitter.minSize > maxRadius)
				{
					maxRadius = emitter.minSize;
				}
			}
			
			radius = maxRadius;
		}

//		Debug.Log(radius);

//		sphere.transform.localScale = Vector3.one * radius;
	}
}

