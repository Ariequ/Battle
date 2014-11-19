using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class PriorityMediatorA : BaseMediator
{
	new public const string NAME = "PriorityMediatorA";

	public PriorityMediatorA () : base(NAME)
	{
	}

	public override IList<string> ListNotificationInterests()
	{
		List<string> notifications = new List<string>();

		RegisterPriorityNotification("PPP", 500);
		
		return notifications;
	}

	public override void HandleNotification(INotification notification)
	{
		switch(notification.Name)
		{
		case "PPP":
			Debug.Log("AAA");
			RemovePriorityNotification("PPP");
			SendPriorityNotification("PPP", null, false);
			break;
		}
	}
}

