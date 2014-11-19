using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RVO;

public class CharacterMoveController : AbstractAnimationController {

	[Range(0.01f, 0.1f)]
	public float positionRefreshInterval = 0.066f;

	private float lastPositionRefreshTime;

//	private GameObjectSnapshotAgent snapshotAgent;
//
//	private GameSceneDataManager sceneDataManager;

	private RvoAgent rvoAgent;

	private Simulator simulator;

	private float radius = 0;

	private String rvoType;

	private String rvoName;

	private bool nearStopDistance;

	private RVO.Vector2 targetPosition;

	private RVO.Vector2 _position_ = new RVO.Vector2();

	private RVO.Vector2 _velocity_ = new RVO.Vector2();

	void Start ()
	{
		GameObject gameController = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER);

//		this.snapshotAgent = GetComponent<GameObjectSnapshotAgent>();
//		this.sceneDataManager = gameController.GetComponent<GameSceneDataManager>();
		this.rvoAgent = gameController.GetComponent<RvoAgent>();

//		this.radius = sceneDataManager.GetCharacterRadiusByName(gameObject.name);
		this.rvoType = gameObject.layer.ToString();
		this.rvoName = gameObject.name;
	}

	public CharacterMoveController () : base (CharacterAnimationState.Move) { }

	public override void StartAnimation (CharacterAnimationState state, Vector3 actionTarget, Vector3 faceDirection)
	{
		this.targetPosition = new RVO.Vector2(actionTarget.x, actionTarget.z);
		this.nearStopDistance = false;

		if (this.simulator != null)
		{
			rvoAgent.modifyGoal(gameObject, this.rvoName, this.rvoType, this.targetPosition);
		}
		else
		{
			this.simulator = rvoAgent.addGameObject(gameObject, this.rvoName, this.rvoType, this.radius, this.targetPosition);

			base.StartAnimation(state, actionTarget, faceDirection);
		}
	}

	public override void UpdateAnimation ()
	{
		simulator.getPositionAndVelocity(this.rvoName, ref _position_, ref _velocity_);
		
		Vector3 position = transform.position;
		position.x = _position_.x ();
		position.z = _position_.y ();
		transform.position = position;
		transform.localRotation = Quaternion.AngleAxis (Mathf.Rad2Deg * Mathf.Atan2 (- _velocity_.x(), _velocity_.y()), Vector3.down);

		if (RVOMath.absSq (_position_ - this.targetPosition) <= 0.36)
		{
			this.nearStopDistance = true;
			this.lastPositionRefreshTime = this.positionRefreshInterval + BattleTime.deltaTime;
		}
		else
		{
			this.lastPositionRefreshTime += BattleTime.deltaTime;
		}
		
		if (this.lastPositionRefreshTime > this.positionRefreshInterval)
		{
			this.lastPositionRefreshTime = 0;
			
//			snapshotAgent.movementTimestamp = BattleTime.time;
//			sceneDataManager.UpdatePosition(gameObject, position);
		}
	}

	public override void StopAnimation ()
	{
		this.nearStopDistance = true;
		this.simulator = null;

		rvoAgent.removeGameObject(gameObject, this.rvoName, this.rvoType);
		
		base.StopAnimation();
	}

	public override bool IsAnimationRunning ()
	{
		return ! this.nearStopDistance;
	}
}
