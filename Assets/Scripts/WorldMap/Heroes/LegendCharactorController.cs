using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LegendCharactorController : MonoBehaviour
{
    public const string NAME = "LordController";

	private bool mouseDown;
	private bool firstDown;
	private IAnimationController animator;

	public Transform guideEffectParent;
	public Transform charactor;

	private LegendColliderController colliderController;
	
	void Start ()
	{
		animator = charactor.GetComponent<IAnimationController>();
		colliderController = charactor.GetComponent<LegendColliderController>();

		guideEffectParent = GameObject.Find("GuideEffects").transform;
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			onMouseDown ();
		}
		else if (Input.GetMouseButton (0))
		{
			onMouseMove ();
		}
		else if (Input.GetMouseButtonUp (0))
		{
			onMouseUp ();
		}
	}

	private void onMouseDown ()
	{
	}

	private void onMouseMove ()
	{
	}

	private void onMouseUp ()
	{
		if (Camera.main != null)
		{
			Vector3 mousePosition = Input.mousePosition;
			RaycastHit hitInfo;
			RaycastHit uiHitInfo;

			Ray ray = Camera.main.ScreenPointToRay (mousePosition);
			
			if (Physics.Raycast (ray, out hitInfo) && !UICamera.Raycast(mousePosition, out uiHitInfo))
			{
				if (colliderController.IsInsideCollider(hitInfo.point))
				{
					colliderController.ReTriggerCollider();
				}
				else
				{
					MoveTo(hitInfo.point);
				}
			}
		}
	}

    public void MoveTo(Vector3 position)
    {
		animator.MoveTo (position);

        GameObject guideEffect = ResourceFacade.Instance.LoadPrefab(PrefabType.WorldMap, "Effects/Others/GuideEffect");
        guideEffect.transform.position = position;
		guideEffect.transform.parent = guideEffectParent;
    }
}
