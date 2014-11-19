using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{
	public class GameMessageRoute {
		
		private Dictionary<String, Dictionary<IMessageDriven, IMessageDriven>> messageBus 
			= new Dictionary<string, Dictionary<IMessageDriven, IMessageDriven>>();
		
		private List<MessageContext> pendingContexts = new List<MessageContext>();
		
		internal void Tick (float deltaTime)
		{
			lock (this.pendingContexts) 
			{
				for (int i = 0; i < pendingContexts.Count; i++)
				{
					MessageContext context = pendingContexts[i];
					
					if (context.CheckDueTime(deltaTime))
					{
						pendingContexts.RemoveAt(i--);
						
						Dictionary<IMessageDriven, IMessageDriven> pool = null;
						messageBus.TryGetValue(context.messageType, out pool);
						
						if (pool != null)
						{
							foreach (IMessageDriven messageDriven in pool.Values)
							{
								messageDriven.OnMessageArrived(context);
							}
						}
					}
				}
			}
		}
		
		public void SendGameMessage (MessageContext context)
		{
			lock(this.pendingContexts)
			{
				pendingContexts.Add(context);
			}
		}
		
		public void RegistryMessageDrivenBehaviour (IMessageDriven behaviour)
		{
			String[] messageTypes = behaviour.GetMessageTypes();
			
			if (messageTypes != null)
			{
				foreach (String messageType in messageTypes)
				{
					Dictionary<IMessageDriven, IMessageDriven> pool;
					messageBus.TryGetValue (messageType, out pool);
					
					if (pool == null) 
					{
						pool = new Dictionary<IMessageDriven, IMessageDriven>();
						pool.Add(behaviour, behaviour);
						
						messageBus.Add(messageType, pool);
					}
					else if (!pool.ContainsKey(behaviour))
						pool.Add(behaviour, behaviour);
				}
			}
			else
			{
				Debug.LogWarning("There is no message types declaration at Message Driven Behaviour " + behaviour);
			}
		}
		
		public void UnregistryMessageDrivenBehaviour (IMessageDriven behaviour)
		{
			String[] messageTypes = behaviour.GetMessageTypes();
			
			if (messageTypes != null)
			{
				foreach (String messageType in messageTypes)
				{
					Dictionary<IMessageDriven, IMessageDriven> pool;
					messageBus.TryGetValue (messageType, out pool);
					
					if (pool != null) 
					{
						pool.Remove(behaviour);
						
						if (pool.Count == 0)
							messageBus.Remove(messageType);
					}
				}
			}
		}
	}
}