using UnityEngine;
using System;
using System.Collections;

public class CharacterAttackDistanceController : DirectionAnimationController
{
	public int bulletID;

	public float shootDelay = 0.2f;

	public GameObject beforeEffect;

	public Transform shootTransform;

	public float distanceThreshold = 1f;

	private AnimationState _nearShootState;

	private AnimationState _farShootState;

	private GameElementManager _elementManager;

    private UnitController unitController;

	private ParticleSystem[] _beforeEffectParticles;

	void Awake ()
	{
		_elementManager = BattleGlobal.elementManager;

//        unitController = GetComponent<UnitController>();
		_beforeEffectParticles = beforeEffect != null ? beforeEffect.GetComponentsInChildren<ParticleSystem>() : null;
	}

	public CharacterAttackDistanceController () : base (CharacterAnimationState.Attack) { }

	public override void StartAnimation (CharacterAnimationState state, Vector3 actionTarget, Vector3 faceDirection)
	{
		float distance = MathUtil.DistanceXZ(shootTransform.position, actionTarget);
		SetAnimationState(distance > distanceThreshold ? 0 : 1);

		base.StartAnimation(state, actionTarget, faceDirection);

		StartCoroutine(ShootBullet(actionTarget, distance));
	}

	private IEnumerator ShootBullet (Vector3 enemyPosition, float distance)
	{
		yield return new WaitForSeconds(shootDelay);

		BulletController bullet = _elementManager.GetElement(ElementType.Effect, bulletID).GetComponent<BulletController>();
		
		if (bullet != null)
		{
			if (_beforeEffectParticles != null)
			{
				foreach (ParticleSystem particleSystem in _beforeEffectParticles)
				{
					particleSystem.Play();
				}
			}

//			bullet.Initialize(shootTransform.position, enemyPosition, distance);
		}
	}
}
