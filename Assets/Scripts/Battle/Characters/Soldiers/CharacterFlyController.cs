using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterFlyController : AbstractAnimationController
{
	[Range(-0.5f, -6.0f)]
	public float disappearPositionY = - 0.5f;

	public List<string> fallAnimationStateList;

	private AnimationState _fallAnimationState;

	private ElementController _elementController;
	
	private bool _hitGround;

	private float _timeStamp;

	void Awake ()
	{
		_elementController = GetComponent<ElementController>();

		_fallAnimationState = animation[fallAnimationStateList[0]];

		_hitGround = false;
	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.CompareTag(Tags.TERRAIN) && !_hitGround)
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			rigidbody.useGravity = false;

			_hitGround = true;

			Vector3 position = transform.position;
			position.y = 0;

			transform.position = position;
			transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

			animation.CrossFade(_fallAnimationState.name);

			_timeStamp = BattleTime.time + BattleTime.deltaTime;
		}
	}

	public CharacterFlyController () : base (CharacterAnimationState.Fly) { }

	public override void UpdateAnimation ()
	{
		if (transform.position.y < disappearPositionY)
		{
			_elementController.Recycle();
			
			_hitGround = false;
		}
		else if (!(_fallAnimationState.time > 0 && _fallAnimationState.time < _fallAnimationState.length || BattleTime.time < _timeStamp))
		{
			rigidbody.useGravity = true;
		}
	}

	public override bool IsAnimationRunning ()
	{
		return true;//Don't destroy this, because it is cached in a object-pool.
	}
}
