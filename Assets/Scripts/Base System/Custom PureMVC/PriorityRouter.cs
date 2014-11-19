using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Interfaces;

namespace PureMVC.Patterns
{
	internal class PriorityRouter
	{
		//按照notificationName分类存储对其感兴趣的mediatorItem（包括mediator和它感兴趣的优先级）
		private Dictionary<string, List<PriorityMediatorItem>> mediatorItemDic;

		public PriorityRouter ()
		{
			mediatorItemDic = new Dictionary<string, List<PriorityMediatorItem>>();
		}

		public void Dispose ()
		{
			foreach (List<PriorityMediatorItem> mediatorItemList in mediatorItemDic.Values)
			{
				mediatorItemList.Clear();
			}
			mediatorItemDic.Clear();
		}

		public void RegisterMediator(IMediator mediator, string notificationName, int priority)
		{
			List<PriorityMediatorItem> mediatorItemList = null;
			mediatorItemDic.TryGetValue(notificationName, out mediatorItemList);

			if (mediatorItemList == null)
			{
				mediatorItemList = new List<PriorityMediatorItem>();
				mediatorItemDic[notificationName] = mediatorItemList;
			}

			//按照优先级从大到小的顺序插入列表中
			PriorityMediatorItem mediatorItem = new PriorityMediatorItem(mediator, priority);
			insertItemInOrder(mediatorItemList, mediatorItem);
		}

		public void RemoveMediator(IMediator mediator, string notificationName)
		{
			List<PriorityMediatorItem> mediatorItemList = null;
			mediatorItemDic.TryGetValue(notificationName, out mediatorItemList);

			if (mediatorItemList != null)
			{
				int count = mediatorItemList.Count;
				
				for (int i = 0; i < count; ++i)
				{
					PriorityMediatorItem item = mediatorItemList[i];
					if (item.mediator == mediator)
					{
						mediatorItemList.RemoveAt(i);
						break;
					}
				}
			}
		}

		public void SendNotification(INotification notification, bool isStucked = true)
		{
			List<PriorityMediatorItem> mediatorItemList = null;
			mediatorItemDic.TryGetValue(notification.Name, out mediatorItemList);

			if (mediatorItemList != null)
			{
				PriorityMediatorItem[] cloneItemList = new PriorityMediatorItem[mediatorItemList.Count];
				mediatorItemList.CopyTo(cloneItemList);

				//若阻塞，只调用第一个（优先级最高的）mediator的Handle方法。否则，按照优先级从大到小的顺序依次调用。
				if (isStucked)
				{
					IMediator mediator = cloneItemList[0].mediator;
					mediator.HandleNotification(notification);
				}
				else
				{
					int length = cloneItemList.Length;
					for (int i = 0; i < length; ++i)
					{
						IMediator mediator = cloneItemList[i].mediator;
						mediator.HandleNotification(notification);
					}
				}
			}
		}

		private void insertItemInOrder(List<PriorityMediatorItem> mediatorItemList, PriorityMediatorItem mediatorItem)
		{
			int count = mediatorItemList.Count;
			int i;
			for (i = 0; i < count; ++i)
			{
				PriorityMediatorItem item = mediatorItemList[i];
				if (mediatorItem.priority > item.priority)
					break;
			}
			mediatorItemList.Insert(i, mediatorItem);
		}

		//For test
		private void printMediatorItems(List<PriorityMediatorItem> mediatorItemList)
		{
			foreach (PriorityMediatorItem item in mediatorItemList)
			{
				Debug.Log(item.mediator.ToString() + "--" + item.priority);
			}
		}
	}
}
