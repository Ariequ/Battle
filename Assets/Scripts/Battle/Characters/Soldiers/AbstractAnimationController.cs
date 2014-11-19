using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractAnimationController : MonoBehaviour {

	public List<string> animationStateList;

	protected bool isRunningCurrentState;

	protected float firstCheckTimestamp;

	protected AnimationState animationState;

	private CharacterAnimationState characterAnimationState;

	private bool crossPlay;

	public AbstractAnimationController (CharacterAnimationState characterAnimationState, bool crossPlay = true)
	{
		this.characterAnimationState = characterAnimationState;
		this.crossPlay = crossPlay;
	}

	public CharacterAnimationState GetAnimationState ()
	{
		return this.characterAnimationState;
	}

	public virtual void Initialize ()
	{
		if (this.animationState == null)
		{
			if (animationStateList.Count == 0)
			{
				Debug.LogError("No animation state name in the list.");
			}

			string defaultStateName = animationStateList[0];

			this.animationState = animation[defaultStateName];
			
			if (this.animationState == null)
			{
				Debug.LogError("Missing animation state '" + defaultStateName + "' for " + transform.name);
			}
		}
	}

	public virtual void StartAnimation (CharacterAnimationState state, Vector3 actionTarget, Vector3 faceDirection)
	{
		this.isRunningCurrentState = true;
		this.firstCheckTimestamp = BattleTime.time + animationState.length;

		if (this.crossPlay)
		{
			animation.CrossFade(animationState.name);
		}
		else
		{
			animation.Play(animationState.name);
		}
	}
	
	public virtual void StopAnimation ()
	{
		this.isRunningCurrentState = false;
	}

	public virtual void UpdateAnimation ()
	{
	}
	
	public virtual bool IsAnimationRunning ()
	{
		return BattleTime.time < this.firstCheckTimestamp;
	}

	protected void SetAnimationState(int index)
	{
		animationState = animation[animationStateList[index]];
	}
}
