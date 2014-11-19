using UnityEngine;
using System.Collections;

public class CharacterIdleController : AbstractAnimationController {

	public CharacterIdleController () : base (CharacterAnimationState.Idle) { }

	public override bool IsAnimationRunning ()
	{
		return true;
	}
}
