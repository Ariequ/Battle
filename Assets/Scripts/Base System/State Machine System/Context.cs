using System;
using System.Collections.Generic;

namespace StateMachine
{
	public class Context
	{
		protected Dictionary<string, object> dataDic;

		public Context ()
		{
			dataDic = new Dictionary<string, object>();
		}

		public void AddData(string key, object data)
		{
			if (!dataDic.ContainsKey(key))
			{
				dataDic[key] = data;
			}
		}

		public object GetData(string key)
		{
			return dataDic.ContainsKey(key) ? dataDic[key] : null;
		}
	}
}