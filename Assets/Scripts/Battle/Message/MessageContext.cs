using System;
using System.Collections.Generic;

namespace Battle
{
	public class MessageContext {
		
		public String messageType;
		
		public int innerType;
		
		private Dictionary<String, System.Object> data;
		
		private float delayTime;
		
		public MessageContext (String messageType, int innerType, Dictionary<String, System.Object> data, float delayTime = 0)
		{
			this.messageType = messageType;
			this.innerType = innerType;
			this.data = data;
			this.delayTime = delayTime;
		}
		
		public bool ContainsKey (String key)
		{
			return data.ContainsKey(key);
		}
		
		public System.Object GetConetextValue (String key)
		{
			System.Object value = null;
			
			if (this.data != null)
				data.TryGetValue(key, out value);
			
			return value;
		}
		
		public bool CheckDueTime (float elapsedTime)
		{
			this.delayTime -= elapsedTime;
			
			return this.delayTime <= 0;
		}
	}
}
