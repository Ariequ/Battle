using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;

public class MessageRouteMediator : Mediator
{
    public new const string NAME = "MessageRouter";


    public override IList<string> ListNotificationInterests()
    {
        List<string> list = new List<string>();
        list.Add(MessageType.SHOW_SCENEOBJECT_PANEL);
        list.Add(MessageType.SHOW_BUILDING_PANEL);
        return list;
    }

    public override void HandleNotification(PureMVC.Interfaces.INotification notification)
    {
        switch (notification.Name)
        {
            case MessageType.SHOW_SCENEOBJECT_PANEL:
            {
                break;
            }
            case MessageType.SHOW_BUILDING_PANEL:
            {
                MainCityBuildingVO buildingVO = notification.Body as MainCityBuildingVO;
                SendNotification(NotificationConst.MAIN_CITY_INIT_PANEL, buildingVO.meta.metaName);
                break;
            }
            default:
                break;
        }
    }
}
