using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{
	[AddComponentMenu("Character/Battle Character Controller")]
	public class BattleCharacterController : ICharacterController
	{
		private enum DeactiveState
		{
			PENDDING,
			DISAPPEAR,
			FLYING,
			DESTROY
		}
		;

		[Range(-0.6f, -3.0f)]
		public float
			disappearDepth = - 1.0f;
		public float disappearSpeed = 1.0f;
		public string idleAnimationClip = "idle";
		public string moveAnimationClip = "run";
		public string attackedAnimationClip = "hit";
		public string dieAnimationClip = "die";
		public string[] attackAnimationClips = new string[] {
			"attack01",
			"attack02"
		};
		public string[] explodeAnimationClips = new string[] { "fly", "fall" };
		public CharacterAttackingContext attackingContext;
		private CharacterState characterState;
		private DeactiveState deactiveState;
		private ElementController elementController;
		private IAttackJubgement attackJubgement;
		private ParticleSystem[] attackEffectParticles;
		private SwiftShadow swiftShadow;
		private Dictionary<string, float> attackAnimationLength = new Dictionary<string, float> ();
		private float lastActionTimestamp;

		void Awake()
		{
			this.swiftShadow = GetComponentInChildren<SwiftShadow>();

			if (attackingContext.attackTransform == null)
			{
				attackingContext.attackTransform = transform;
			}
		}

		void Start()
		{
			this.elementController = GetComponent<ElementController>();
			this.attackJubgement = GetComponent<IAttackJubgement>();
			this.attackEffectParticles = attackingContext.attackEffect != null ? attackingContext.attackEffect.GetComponentsInChildren<ParticleSystem>() : null;

			for (int i = 0; i < attackAnimationClips.Length; i++)
			{
				string animationName = attackAnimationClips [i];
				AnimationState animationState = animation [animationName];
				attackAnimationLength.Add(animationName, animationState.length);
			}

			initializeAnimationSpeed(BattleTime.timeScale);
		}

		void OnEnable()
		{
			this.characterState = CharacterState.None;
			this.deactiveState = DeactiveState.PENDDING;

			if (swiftShadow != null)
			{
				swiftShadow.enabled = true;
			}
		}

		void Update()
		{
			if (DeactiveState.DISAPPEAR == deactiveState)
			{
				Vector3 position = transform.position;
				position.y = Mathf.Lerp(position.y, disappearDepth - 0.5f, BattleTime.deltaTime * this.disappearSpeed);
				
				if (position.y < this.disappearDepth)
				{
					if (this.elementController != null && elementController.enabled)
					{
						elementController.Recycle();
					}
					else
					{
						this.characterState = CharacterState.Destroy;
					}
				}
				else
				{
					transform.position = position;
				}
			}
		}
		
		void OnTriggerEnter(Collider collider)
		{
			if (DeactiveState.FLYING == deactiveState && collider.CompareTag(Tags.TERRAIN))
			{
				this.deactiveState = DeactiveState.PENDDING;

				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				rigidbody.useGravity = false;
				
				Vector3 position = transform.position;
				position.y = 0;
				
				transform.position = position;
				transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

				string fallAnimationName = explodeAnimationClips [1];
				animation.CrossFade(fallAnimationName);

				AnimationState animationState = animation [fallAnimationName];
				StartCoroutine(disappear(animationState.length));
			}
		}

		public override CharacterState GetAnimationState()
		{
			return this.characterState;
		}

		public override void Attacked()
		{
			this.lastActionTimestamp = BattleTime.time;

			animation.CrossFade(attackedAnimationClip);
			characterState = CharacterState.Attacked;
		}
		
		public override void Attack(GameObject enemy, int attackMode, float attackFrequency)
		{
			this.lastActionTimestamp = BattleTime.time;

			Transform enemyTransform = enemy.transform;
			float distance = MathUtil.DistanceXZ(attackingContext.attackTransform.position, enemyTransform.position);
			string animationClipName = this.attackJubgement != null 
				? attackJubgement.JubgeAttackStyle(this.attackAnimationClips, attackingContext.attackTransform, enemyTransform, distance, attackMode) 
					: attackAnimationClips [RandomUtil.Range(0, attackAnimationClips.Length)];
			float animationClipLength = 1.0f;
			attackAnimationLength.TryGetValue(animationClipName, out animationClipLength);

			animation.CrossFade(animationClipName);
			characterState = CharacterState.Attack;

			if (this.attackEffectParticles != null)
			{
				foreach (ParticleSystem particleSystem in this.attackEffectParticles)
				{
					particleSystem.Play();
				}
			}

			if (attackFrequency > animationClipLength)
			{
				StartCoroutine(playAttackPadding(this.lastActionTimestamp, animationClipLength, this.idleAnimationClip, attackFrequency - animationClipLength));
			}
		}
		
		public override void Idle(Vector3 direction)
		{
			this.lastActionTimestamp = BattleTime.time;

			animation.CrossFade(idleAnimationClip);
			characterState = CharacterState.Idle;
		}

		public override void Move()
		{
			this.lastActionTimestamp = BattleTime.time;

			animation.CrossFade(moveAnimationClip);
			characterState = CharacterState.Move;
		}
		
		public override void Die()
		{
			this.lastActionTimestamp = BattleTime.time;

			if (gameObject != null)
			{
				animation.CrossFade(dieAnimationClip);
				characterState = CharacterState.Die;
				
				AnimationState animationState = animation [dieAnimationClip];
				StartCoroutine(disappear(animationState.length));
			}
		}
		
		public override void ExplodeAway(ExplosionInfo explosionInfo)
		{
			Vector3 direction = (transform.position - explosionInfo.transform.position).normalized;
			
			float explosionForce = RandomUtil.Range(explosionInfo.maxForce, explosionInfo.minForce);
			float explosionTorque = RandomUtil.Range(explosionInfo.maxTorque, explosionInfo.minTorque);

			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
			rigidbody.AddForce(direction * explosionForce);
			rigidbody.AddTorque(UnityEngine.Random.onUnitSphere * explosionTorque);
			
			collider.enabled = true;
			collider.isTrigger = true;
			
			animation.CrossFade(explodeAnimationClips [0]);
			characterState = CharacterState.Die;

			StartCoroutine(flyAway(0.5f));
		}

		public override CharacterAttackingContext AttackingContext
		{ 
			get
			{
				return this.attackingContext;
			}
		}

		private IEnumerator playAttackPadding(float actionTimestamp, float delay, string paddingAnimation, float paddingSeconds)
		{
			yield return new WaitForSeconds (delay / BattleTime.timeScale);

			if (this.lastActionTimestamp == actionTimestamp)
			{
				animation.CrossFade(paddingAnimation);

				StartCoroutine(finishAttack(actionTimestamp, paddingSeconds));
			} 
//			else if (gameObject.name == "0_3_Opponent")
//			{
//				Debug.LogWarning(">> " + (this.lastActionTimestamp - actionTimestamp).ToString());
//			}
		}

		private IEnumerator finishAttack(float actionTimestamp, float delay)
		{
			yield return new WaitForSeconds (delay / BattleTime.timeScale);

			if (this.lastActionTimestamp == actionTimestamp)
			{
				this.characterState = CharacterState.Idle;
			}
//			else if (gameObject.name == "0_3_Opponent")
//			{
//				Debug.LogWarning(">> " + (this.lastActionTimestamp - actionTimestamp).ToString());
//			}
		}
		
		private IEnumerator disappear(float delay)
		{
			yield return new WaitForSeconds (delay / BattleTime.timeScale);

			this.deactiveState = DeactiveState.DISAPPEAR;

			if (swiftShadow !=null && swiftShadow.enabled)
			{
				swiftShadow.enabled = false;
			}
		}

		private IEnumerator flyAway(float delay)
		{
			yield return new WaitForSeconds (delay / BattleTime.timeScale);
			
			this.deactiveState = DeactiveState.FLYING;
		}

		private void initializeAnimationSpeed(float speed)
		{
			animation [idleAnimationClip].speed = speed;
			animation [moveAnimationClip].speed = speed;
			animation [attackedAnimationClip].speed = speed;
			animation [dieAnimationClip].speed = speed;
			;

			for (int i = 0; i < attackAnimationClips.Length; i++)
			{
				animation [attackAnimationClips [i]].speed = speed;
			}

			for (int i = 0; i < explodeAnimationClips.Length; i++)
			{
				AnimationState animationState = animation [explodeAnimationClips [i]];

				if (animationState != null)
				{
					animationState.speed = speed;
				}
			}
		}
	}
}
