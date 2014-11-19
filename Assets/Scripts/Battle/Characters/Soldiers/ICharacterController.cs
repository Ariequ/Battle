using UnityEngine;
using System;
using System.Collections;

namespace Battle
{
	[Serializable]
	public class CharacterAttackingContext
	{
		public Transform attackTransform;
		
		public float attackDelay;
		
		public GameObject attackEffect;
		
		public Transform attackedTransform;
	}

	public abstract class ICharacterController : MonoBehaviour 
	{
		public abstract CharacterState GetAnimationState ();

		public abstract void Attacked ();
		
        public abstract void Attack (GameObject enemy, int attackMode, float attackFrequency);
		
		public abstract void Idle (Vector3 direction);
		
		public abstract void Move ();
		
		public abstract void Die ();
		
		public abstract void ExplodeAway (ExplosionInfo explosionInfo);

		public abstract CharacterAttackingContext AttackingContext { get; }
	}
	
	public enum CharacterState 
	{
		None,
		Idle,
		Move,
		Attack,
		Attacked,
		Die,
		Destroy,
	}
}
