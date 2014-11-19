using UnityEngine;
using System.Collections;

public class CharacterDieController : AbstractAnimationController {
	
	public CharacterDieController () : base (CharacterAnimationState.Die, false) { }

	public override void UpdateAnimation ()
	{
		if (transform.position.y > 0)
		{
			Vector3 position = transform.position;
			position.y = Mathf.Lerp(position.y, 0, BattleTime.deltaTime * 0.5f);
			transform.position = position;
		}
	}
}
