using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class DirectionAnimationController : AbstractAnimationController {

	[Range(1f, 10f)]
	public float turnSmoothing = 5f;	// A smoothing value for turning the player.

	[Range(5, 15)]
	public int minAttackRotation = 10;
	
	private Quaternion lookatRotation = Quaternion.identity;

	public DirectionAnimationController (CharacterAnimationState characterAnimationState, bool crossPlay = true) : base (characterAnimationState, crossPlay) { }

	public override void StartAnimation (CharacterAnimationState state, Vector3 actionTarget, Vector3 faceDirection)
	{
		if (Vector3.zero != faceDirection)
		{
			faceDirection.y = 0;

			this.lookatRotation = Quaternion.LookRotation(faceDirection, Vector3.up);

			if (Mathf.Abs(Quaternion.Angle(this.lookatRotation, transform.rotation)) < minAttackRotation)
			{
				this.lookatRotation = Quaternion.identity;
			}
		}
		else
		{
			this.lookatRotation = Quaternion.identity;
		}
		
		base.StartAnimation(state, actionTarget, faceDirection);
	}

	public override void UpdateAnimation ()
	{
		if (Quaternion.identity != this.lookatRotation)
		{
			Quaternion newRotation = Quaternion.Lerp(transform.rotation, this.lookatRotation, this.turnSmoothing * BattleTime.deltaTime);
			
			if (transform.rotation == newRotation || this.lookatRotation == newRotation)				// TODO ???
			{
				transform.rotation = this.lookatRotation;
			}
			else
			{
				transform.rotation = newRotation;
				
				return;
			}
			
			this.lookatRotation = Quaternion.identity;
		}
	}
}