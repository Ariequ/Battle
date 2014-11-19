using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class IAnimationController : MonoBehaviour {

	public abstract Vector3 GetForwradDirection ();

	public abstract CharacterAnimationState GetAnimationState ();
	
	public abstract CharacterAnimationState Attacked ();
	
	public abstract CharacterAnimationState Attack (Vector3 enemyPosition, int skillType, bool isCritical);
	
	public abstract CharacterAnimationState Idle (Vector3 direction);
	
	public abstract CharacterAnimationState MoveTo (Vector3 destination);
	
	public abstract CharacterAnimationState Die ();

	public abstract CharacterAnimationState ExplodeAway (ExplosionInfo explosionInfo);
}
