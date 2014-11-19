using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIUtil
{
	public static void AddOrModifyExistKey(Dictionary<string,object> dictionarySource, string Key, object Value)
	{
		if (dictionarySource == null)
		{
			return;
		}

		if (dictionarySource.ContainsKey(Key))
		{
			dictionarySource[Key] = Value;
		}
		else
		{
			dictionarySource.Add(Key,Value);
		}
	}

	public static void Log(string log) 
	{
//		Debug.Log(log);
	}

}
