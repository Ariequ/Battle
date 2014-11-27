using UnityEngine;
using System.Collections;

public class NOTCondition : Condition
{
	private IBehaviorCondition _target = null;

	public  NOTCondition(IBehaviorCondition target)
	{
		_target = target;
	}
	
    override public bool isConditionTrue(IAIContext context)
	{
		return _target == null || !_target.isConditionTrue(context);
	}
}
