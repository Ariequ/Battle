using UnityEngine;
using System;

public class LogUtility {

	public enum Level
	{
		None,
		Error,
		Exception,
		Warning,
		Message,
	};

	public static Level level = Level.Message;

	public void Log (System.Object message)
	{
		if (level >= Level.Message)
		{
			Debug.Log(message);
		}
	}

	public void LogWarning (System.Object warnning)
	{
		if (level >= Level.Warning)
		{
			Debug.LogWarning(warnning);
		}
	}
	
	public void LogException (System.Exception exception)
	{
		if (level >= Level.Exception)
		{
			Debug.LogException(exception);
		}
	}

	public void LogError (System.Object error)
	{
		if (level >= Level.Error)
		{
			Debug.LogError(error);
		}
	}
}
