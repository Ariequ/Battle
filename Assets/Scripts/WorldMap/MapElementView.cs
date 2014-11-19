using System;
using UnityEngine;
using PureMVC.Patterns;

public class MapElementView : MonoBehaviour
{
    public MapElementVO mapElementVO;
    private bool charactorNear;

    public void OnMouseUp()
    {
//      ApplicationFacade.Instance.SendNotification(NotificationConst.LORD_MOVE, transform.position);

//      	Debug.Log(mapElementVO.meta.id);
        if (charactorNear)
        {
            if (mapElementVO.meta.type == MapElementType.Building)
            {
                ApplicationFacade.Instance.SendNotification(NotificationConst.EXPLORE_BUILDING_SHOW, mapElementVO.meta.id);
            }
            else if(mapElementVO.meta.type == MapElementType.Monster)
            {
                MockServer.worldVO.inBattleMonsterID = mapElementVO.ID;
                LevelProxy levelProxy = ApplicationFacade.Instance.RetrieveProxy(LevelProxy.NAME) as LevelProxy;
                ApplicationFacade.Instance.SendNotification (NotificationConst.BATTLE_PREPARATION_SHOW, levelProxy.GetLevelVO(mapElementVO.ID));
            }
            else if(mapElementVO.meta.type == MapElementType.Treasure)
            {
                MockServer.worldVO.defeatedTreasureIDList.Add (mapElementVO.ID);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        charactorNear = true;

        if (mapElementVO.meta.type == MapElementType.Building)
        {
            renderer.material.color = Color.yellow;
        }
        else if (mapElementVO.meta.type == MapElementType.Monster)
        {
            animation.Play("attack01");
        }
    }

    void OnTriggerExit(Collider other)
    {
        charactorNear = false;

        if (mapElementVO.meta.type == MapElementType.Building)
        {
            renderer.material.color = Color.white;
        }

        else if (mapElementVO.meta.type == MapElementType.Monster)
        {
            animation.Play("idle");
        }
    }
}

