using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionController : MonoBehaviour 
{
	public GameObject airExplosion;

	public GameObject landExplosion;

	private ExplosionInfo _explosionInfo;

	void Awake ()
	{
		_explosionInfo = GetComponent<ExplosionInfo>();

		ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem> ();
		
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			particleSystem.playbackSpeed = BattleTime.timeScale;
		}
	}

	void Start ()
	{
		bool isLand = transform.position.y <= _explosionInfo.radius;

		airExplosion.SetActive(!isLand);
		landExplosion.SetActive(isLand);
	}

	public void Prepare (bool airAttack)
	{
		airExplosion.SetActive (airAttack);
		landExplosion.SetActive (!airAttack);
	}
}
