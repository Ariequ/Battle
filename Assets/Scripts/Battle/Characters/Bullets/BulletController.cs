using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Battle;

public class BulletController : MonoBehaviour
{
	[HideInInspector]
	public ElementController elementController;

	private GameSceneEffectManager effectManager;
	private Faction faction;
	private BulletMeta bulletMeta;
	private Vector3 from, to;
	private List<SoldierMeta> detectedEnemys;

	private float _currentSpeed;
	private float _distance;
	private Vector3 _middlePosition;
	
	private float _t;

	void Awake ()
	{
		this.elementController = GetComponent<ElementController>();

		ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem> ();
		
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			particleSystem.playbackSpeed = BattleTime.timeScale;
		}
	}

	void Update ()
	{
		if (this.bulletMeta != null)
		{
			_currentSpeed = _currentSpeed > bulletMeta.minSpeed ? _currentSpeed - bulletMeta.drag : bulletMeta.minSpeed;
			_t += _currentSpeed * BattleTime.deltaTime / _distance;
			
			if (_t < 1.0f)
			{
				Vector3 direction;
				
				if (_distance > BulletMeta.MIN_SHOOT_DISTANCE)
				{
					transform.position = BezierUtil.LerpPosition(from, _middlePosition, to, _t);
					direction = BezierUtil.LerpDirection(from, _middlePosition, to, _t);
				}
				else
				{
					transform.position = BezierUtil.LerpPosition(from, to, _t);
					direction = BezierUtil.LerpDirection(from, to);
				}
				
				transform.localRotation = Quaternion.LookRotation(direction);
			}
			else
			{
				explode(null);
			}
		}
	}

	void OnTriggerEnter (Collider collider)
	{
		if (this.bulletMeta != null)
		{
			string selfTag = FactionUtil.ParseToTag(faction);

			if (!collider.CompareTag(selfTag))
			{
				explode(collider.name);
			}
		}
	}

	public void Initialize (GameSceneEffectManager effectManager, BulletMeta bulletMeta, Faction faction, Vector3 from, Vector3 to, List<SoldierMeta> detectedEnemys)
	{
		this.effectManager = effectManager;
		this.bulletMeta = bulletMeta;
		this.faction = faction;
		this.from = from;
		this.to = to;
		this.detectedEnemys = detectedEnemys;

		_t = 0;
		_currentSpeed = bulletMeta.maxSpeed;
		_distance = Vector3.Distance(from, to);
		_middlePosition = BezierUtil.GetMiddlePosition(from, to, _distance);

		transform.position = from;
	}
	
	private void explode (string hitName)
	{
		if (bulletMeta.isAreaAttack)
		{
			effectManager.ShowAreaAttackingEffect(this.bulletMeta, this.faction, transform.position, hitName, this.detectedEnemys);
		}
		
		this.bulletMeta = null;

		elementController.Recycle();
	}
}
