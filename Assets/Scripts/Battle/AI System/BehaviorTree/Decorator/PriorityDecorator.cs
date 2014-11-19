using UnityEngine;
using System.Collections;

public class PriorityDecorator : Decorator
{
	public int getPriority ()
	{
		return int.Parse (this.getParam() ["priority"]);
	}

	public int getWeight ()
	{
		return int.Parse (this.getParam() ["weight"]);
	}
}
