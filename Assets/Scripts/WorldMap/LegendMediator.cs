using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class LegendMediator : Mediator
{
    new public const string NAME = "WorldMapMediator";

    private LegendCharactorController lordController;

    public LegendMediator() : base(NAME)
    {
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();

        notifications.Add(NotificationConst.WORLDMAP_READY);
        notifications.Add(NotificationConst.LORD_MOVE);
        
        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name)
        {
            case NotificationConst.WORLDMAP_READY:
            {
                worldMapReadyHandler();
                break;
            }
            case NotificationConst.LORD_MOVE:
            {
                Vector3 position = (Vector3)(notification.Body);
                handleLordMove(position);
                break;
            }
        }
    }

    private void worldMapReadyHandler()
    {
        lordController = GameObject.Find(LegendCharactorController.NAME).GetComponent<LegendCharactorController>();
    }

    private void handleLordMove(Vector3 position)
    {
        lordController.MoveTo(position);
    }

}

