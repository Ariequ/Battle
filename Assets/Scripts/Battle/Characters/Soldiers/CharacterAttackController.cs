using UnityEngine;
using System.Collections;

public class CharacterAttackController : DirectionAnimationController {

	public CharacterAttackController () : base (CharacterAnimationState.Attack) { }

	public override void StartAnimation (CharacterAnimationState state, Vector3 actionTarget, Vector3 faceDirection)
	{
		SetAnimationState(RandomUtil.Range(0, animationStateList.Count));

		base.StartAnimation(state, actionTarget, faceDirection);
	}
}
