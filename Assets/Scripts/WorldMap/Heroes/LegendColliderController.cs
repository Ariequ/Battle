using UnityEngine;
using System.Collections;

using PureMVC.Patterns;

public class LegendColliderController : MonoBehaviour
{
	private IAnimationController animationController;

    private Collider otherCollider;

	void Start ()
	{
		animationController = GetComponent<IAnimationController> ();
	}

	void OnDestroy ()
	{
		MockServer.RecordLordTransform(transform);
	}

	void OnTriggerEnter (Collider other)
	{
		//避免反复触发
		if (Layers.EXPLORE_ELEMENTS == other.gameObject.layer && other != this.otherCollider)
        {
            this.otherCollider = other;
			animationController.Idle(transform.forward);

			MapElementView mapElementView = other.GetComponent<MapElementView>();

			if (mapElementView != null && mapElementView.mapElementVO != null)
			{
				triggerElementEvent(mapElementView.mapElementVO);
			}
        }
	}

    void OnTriggerExit (Collider other)
    {
        //避免反复触发
        this.otherCollider = null;
    }

	public bool IsInsideCollider (Vector3 targetPosition)
	{
		if (this.otherCollider != null)
		{
			Vector3 position = transform.position;
			Vector2 position2 = new Vector2 (position.x, position.z);
			Vector3 collidePosition = otherCollider.transform.position;
			
			Vector2 targetDirection2 = new Vector2 (targetPosition.x, targetPosition.z) - position2;
			Vector2 colliderDirection2 = new Vector2 (collidePosition.x, collidePosition.z) - position2;
			float targetAngle = Vector2.Angle(targetDirection2, colliderDirection2);

			return targetAngle < 45f;
		}
		else
		{
			return false;
		}
	}

	public void ReTriggerCollider ()
	{
		MapElementView mapElementView = otherCollider.GetComponent<MapElementView>();
		
		if (mapElementView != null && mapElementView.mapElementVO != null)
		{
			triggerElementEvent(mapElementView.mapElementVO);
		}
	}

    private void triggerElementEvent(MapElementVO mapElementVO)
    {
//		MapElementMeta meta = mapElementVO.meta;
//
//        switch (meta.type)
//        {
//            case MapElementType.Building:
//                ApplicationFacade.Instance.SendNotification(NotificationConst.EXPLORE_BUILDING_SHOW, meta.id);
//                break;
//            case MapElementType.Monster:
//				MockServer.worldVO.inBatlleMonsterID = mapElementVO.ID;
//                LevelProxy levelProxy = ApplicationFacade.Instance.RetrieveProxy(LevelProxy.NAME) as LevelProxy;
//                Debug.Log(mapElementVO.ID);
//				ApplicationFacade.Instance.SendNotification (NotificationConst.BATTLE_PREPARATION_SHOW, levelProxy.GetLevelVO(mapElementVO.ID));
//                break;
//            case MapElementType.Treasure:
//                break;
//        }
    }
}
