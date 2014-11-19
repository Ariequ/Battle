using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class PriorityMediatorB : BaseMediator
{
	new public const string NAME = "PriorityMediatorB";
	
	public PriorityMediatorB () : base(NAME)
	{
	}
	
	public override IList<string> ListNotificationInterests()
	{
		List<string> notifications = new List<string>();
		
		RegisterPriorityNotification("PPP", 100);
		
		return notifications;
	}
	
	public override void HandleNotification(INotification notification)
	{
		switch(notification.Name)
		{
		case "PPP":
			Debug.Log("BBB");
			break;
		}
	}
}

