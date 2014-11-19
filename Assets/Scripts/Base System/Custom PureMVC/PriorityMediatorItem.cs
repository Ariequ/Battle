using System;
using PureMVC.Interfaces;

namespace PureMVC.Patterns
{
	public struct PriorityMediatorItem
	{
		public IMediator mediator;

		public int priority;

		public PriorityMediatorItem (IMediator mediator, int priority)
		{
			this.mediator = mediator;
			this.priority = priority;
		}
	}
}

