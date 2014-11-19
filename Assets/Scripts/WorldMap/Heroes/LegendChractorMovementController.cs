using UnityEngine;
using System;
using System.Collections;

using Pathfinding;

public class LegendChractorMovementController : AbstractAnimationController
{
	public string turnLeftAnimationName = "turn_left";

	public string turnRightAnimationName = "turn_right";

	[Range(0, 1.0f)]
	public float turnRunningDelay = 0.9f;

	[Range(0.5f, 1.0f)]
	public float slowdownDistance = 0.5f;

	[Range(60, 180)]
	public int minTurnAngle = 90;

	private Transform transformTemp;
	private LegendPathfinder pathAgent;

	private float turnLeftAnimationLength;

	private float turnRightAnimationLength;

	private Vector2 targetPosition = Vector2.zero;

	private Quaternion targetDirection;

	private Vector2 __selfPosition = new Vector2();

	void Awake ()
	{
		this.pathAgent = GetComponent<LegendPathfinder> ();
		this.turnLeftAnimationLength = animation [turnLeftAnimationName].length;
		this.turnRightAnimationLength = animation [turnRightAnimationName].length;
		transformTemp = new GameObject().transform;
	}

	void Start ()
	{
		pathAgent.enabled = true;
	}

	public LegendChractorMovementController () : base (CharacterAnimationState.Move)
	{
	}
	
	public override bool IsAnimationRunning ()
	{
		Vector3 position = transform.position;
		__selfPosition.x = position.x;
		__selfPosition.y = position.z;

		return Vector2.Distance(this.targetPosition, this.__selfPosition) > this.slowdownDistance;
	}

	public override void StartAnimation (CharacterAnimationState state, Vector3 actionTarget, Vector3 faceDirection)
	{
		Vector3 targetDirection = (actionTarget - transform.position);
		targetDirection.y = 0;
		this.targetDirection = Quaternion.LookRotation (targetDirection);
		this.targetPosition = new Vector2(actionTarget.x, actionTarget.z);

		float turnAngle = Quaternion.Angle(this.targetDirection, transform.rotation);
		
		if (turnAngle > this.minTurnAngle) 
		{
			if (Vector3.Cross(targetDirection, transform.forward).y > 0)
			{
				animation.CrossFade (this.turnLeftAnimationName);
				StartCoroutine (TurnPreComplete (this.turnLeftAnimationLength, state, actionTarget, faceDirection));
			}
			else
			{
				animation.CrossFade (this.turnRightAnimationName);
				StartCoroutine (TurnPreComplete (this.turnRightAnimationLength, state, actionTarget, faceDirection));
			}
		}
		else
		{
			this.targetDirection = Quaternion.identity;

			pathAgent.Destination = actionTarget;

			base.StartAnimation (state, actionTarget, faceDirection);
		}
	}

	public override void StopAnimation ()
	{
		this.targetPosition = Vector2.zero;

		pathAgent.Destination = transform.position;

		base.StopAnimation ();
	}

    public override void UpdateAnimation ()
    {
		if (Quaternion.identity != this.targetDirection)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, this.targetDirection, Time.deltaTime * 3f);
		}
    }

	private IEnumerator TurnPreComplete (float delay, CharacterAnimationState state, Vector3 actionTarget, Vector3 faceDirection)
	{
		StartCoroutine (TurnComplete(delay, state, actionTarget, faceDirection));

		yield return new WaitForSeconds (delay * this.turnRunningDelay);

		if (Vector2.zero != this.targetPosition) 
		{
			this.targetDirection = Quaternion.identity;

			pathAgent.Destination = new Vector3(targetPosition.x, actionTarget.y, targetPosition.y);
		}
	}

	private IEnumerator TurnComplete (float delay, CharacterAnimationState state, Vector3 actionTarget, Vector3 faceDirection)
	{
		yield return new WaitForSeconds (delay);

		if (Vector2.zero != this.targetPosition) 
		{
			base.StartAnimation (CharacterAnimationState.None, actionTarget, faceDirection);
		}
	}
}
