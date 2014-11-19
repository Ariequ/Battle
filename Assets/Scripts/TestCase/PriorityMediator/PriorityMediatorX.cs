using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class PriorityMediatorX : BaseMediator
{
	new public const string NAME = "PriorityMediatorX";
	
	public PriorityMediatorX () : base(NAME)
	{
	}
	
	public override IList<string> ListNotificationInterests()
	{
		List<string> notifications = new List<string>();

		notifications.Add("Start");
		
		return notifications;
	}
	
	public override void HandleNotification(INotification notification)
	{
		switch(notification.Name)
		{
		case "Start":
			SendPriorityNotification("PPP", null, false);
			break;
		}
	}
}

