using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestMediator : ChangeableListNotificationMediator
{
    private IList<string> myListNotification;

    public TestMediator() : base("testmediator")
    {
        myListNotification = new List<string>();
        myListNotification.Add("testNotfication");
    }

    public override IList<string> ChangeableListNotification()
    {
        return myListNotification;
    }

    public override void HandleNotification(PureMVC.Interfaces.INotification notification)
    {
        switch (notification.Name)
        {
            case "testNotfication":
                {
                    Debug.Log("testNotfication");
                    myListNotification = new List<string>();
                    myListNotification.Add("testNotfication2");
                    base.ChangeListNotification();
					Facade.SendNotification("testNotfication2");
                    break;
                }
               
            case "testNotfication2":
                {
                    Debug.Log("testNotfication2");
                    break;
                }

            default:
                {
                    break;
                }
        }
    }
}
