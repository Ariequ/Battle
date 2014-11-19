using UnityEngine;
using System.Collections;

public class CharacterDisappearController : AbstractAnimationController {

	[Range(0.2f, 2.0f)]
	public float disappearSpeed = 0.5f;
	
	[Range(-1.0f, -10.0f)]
	public float disappearPositionY = - 1.5f;

	private float measureDisappearPositionY;
	
	public CharacterDisappearController () : base (CharacterAnimationState.Disappear) { }

	public override void Initialize ()
	{
		this.measureDisappearPositionY = disappearPositionY * 2;
	}

	public override void StartAnimation (CharacterAnimationState state, Vector3 actionTarget, Vector3 faceDirection)
	{
		this.isRunningCurrentState = true;

		animation.Stop();
	}

	public override void UpdateAnimation ()
	{
		Vector3 position = transform.position;
		position.y = Mathf.LerpAngle(position.y, this.measureDisappearPositionY, BattleTime.deltaTime * this.disappearSpeed);
		transform.position = position;
	}
	
	public override bool IsAnimationRunning ()
	{
		return transform.position.y > this.disappearPositionY;
	}
}
