using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(LocomotionIdleController))]
//[RequireComponent(typeof(LocomotionMovementController))]
////[RequireComponent(typeof(LocomotionAttackController))]
//[RequireComponent(typeof(LocomotionAttackedController))]
//[RequireComponent(typeof(LocomotionDieController))]
//[RequireComponent(typeof(CharacterDisappearController))]
[AddComponentMenu("Character/Character Animation Controller")]
public class CharacterAnimationController : IAnimationController {

	private Dictionary<CharacterAnimationState, AbstractAnimationController> controllerIndex 
		= new Dictionary<CharacterAnimationState, AbstractAnimationController>();

	private AbstractAnimationController currentAnimationController;
	private CharacterAnimationState currentAnimationState;

	void Awake ()
	{
		AbstractAnimationController[] controllers = GetComponents<AbstractAnimationController>();
		
		foreach (AbstractAnimationController controller in controllers)
		{
			controllerIndex.Add(controller.GetAnimationState(), controller);
		}
	}

	void Start () 
	{
		foreach (AbstractAnimationController controller in controllerIndex.Values)
		{
			if (controller.enabled)
			{
				controller.Initialize();
			}
		}

//		Idle(transform.forward);
	}

	void Update () 
	{
		if (this.currentAnimationController != null)
		{
			if (currentAnimationController.IsAnimationRunning())
			{
				currentAnimationController.UpdateAnimation();
			}
			else
			{
				switch (currentAnimationController.GetAnimationState())
				{
				case CharacterAnimationState.Fly:
				case CharacterAnimationState.Die:
					StartAnimation(CharacterAnimationState.Disappear);
					break;
				case CharacterAnimationState.Disappear:
					transform.parent = null;
					Destroy(gameObject);
					break;
				default:
					Idle(transform.forward);
					break;
				}
			}
		}
	}

	public override Vector3 GetForwradDirection ()
	{
		return transform.localEulerAngles;
	}

	public override CharacterAnimationState GetAnimationState ()
	{
		return this.currentAnimationState; 
	}

	public override CharacterAnimationState Attacked ()
	{
		return StartAnimation(CharacterAnimationState.Attacked);
	}

	public override CharacterAnimationState Attack (Vector3 enemyPosition, int skillType, bool isCritical)
	{
		return StartAnimation(CharacterAnimationState.Attack, enemyPosition, enemyPosition - transform.position);
	}

	public override CharacterAnimationState Idle (Vector3 direction)
	{
		return StartAnimation(CharacterAnimationState.Idle, Vector3.zero, direction);
	}

	public override CharacterAnimationState MoveTo (Vector3 destination)
	{
		return StartAnimation(CharacterAnimationState.Move, destination, Vector3.zero);
	}

	public override CharacterAnimationState Die ()
	{
		return StartAnimation(CharacterAnimationState.Die);
	}

	public override CharacterAnimationState ExplodeAway (ExplosionInfo explosionInfo)
	{
		//Now, this is a fake soldier instantiated by GameElementManager. It's position is set to somewhere around the explosion center.
		Vector3 direction = (transform.position - explosionInfo.transform.position).normalized;

		float explosionForce = RandomUtil.Range(explosionInfo.maxForce, explosionInfo.minForce);
		float explosionTorque = RandomUtil.Range(explosionInfo.maxTorque, explosionInfo.minTorque);
		
		rigidbody.isKinematic = false;
		rigidbody.AddForce(direction * explosionForce);
		rigidbody.AddTorque(Random.onUnitSphere * explosionTorque);

		if (!collider.isTrigger)
		{
			CharacterFlyController characterFlyController = GetComponent<CharacterFlyController>();
			
			characterFlyController.enabled = true;
			characterFlyController.Initialize();
			
//			GetComponent<SoldierAIAgent>().enabled = false;
//			GetComponent<GameSensor>().enabled = false;
//			GetComponent<UnitInfo>().enabled = false;
//			GetComponent<CharacterMoveController>().enabled = false;
			collider.isTrigger = true;
		}

		return StartAnimation(CharacterAnimationState.Fly);
	}

	private CharacterAnimationState StartAnimation (CharacterAnimationState state)
	{
		return StartAnimation(state, Vector3.zero, Vector3.zero);
	}

	private CharacterAnimationState StartAnimation (CharacterAnimationState state, Vector3 actionTarget, Vector3 faceDirection)
	{
		if (this.currentAnimationController == null)
		{
			controllerIndex.TryGetValue(state, out this.currentAnimationController);
		}
		else if (currentAnimationController.GetAnimationState() != state)
		{
			currentAnimationController.StopAnimation();
			controllerIndex.TryGetValue(state, out this.currentAnimationController);
		}

		this.currentAnimationState = CharacterAnimationState.Disappear != state ? state : CharacterAnimationState.Die;
		
		currentAnimationController.StartAnimation(state, actionTarget, faceDirection);

		return state;
	}
}
