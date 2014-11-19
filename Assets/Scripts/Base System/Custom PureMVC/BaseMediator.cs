using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Interfaces;

namespace PureMVC.Patterns
{
	public class BaseMediator : Mediator
	{
		private static PriorityRouter priorityRouter;

		public BaseMediator(string mediatorName): base(mediatorName, null) 
		{ 
			if (priorityRouter == null)
			{
				priorityRouter = new PriorityRouter();
			}
		}

		protected void RegisterPriorityNotification(string notificationName, int priority)
		{
			priorityRouter.RegisterMediator(this, notificationName, priority);
		}

		protected void RemovePriorityNotification(string notificationName)
		{
			priorityRouter.RemoveMediator(this, notificationName);
		}

		protected void SendPriorityNotification(string notificationName, object body = null, bool isStucked = true)
		{
			INotification notification = new Notification(notificationName, body);
			priorityRouter.SendNotification(notification, isStucked);
		}
	}
}

