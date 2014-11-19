using UnityEngine;
using System.Collections;

public class ANDCondition : Condition {
	private IBehaviorCondition _targetA = null;
	private IBehaviorCondition _targetB = null;

	public ANDCondition(IBehaviorCondition targetA,IBehaviorCondition targetB)
	{
		_targetA = targetA;
		_targetB = targetB;
	}
	
    override public bool isConditionTrue(IAIContext context)
	{
		return (_targetA != null && _targetA.isConditionTrue(context)) && (_targetB != null && _targetB.isConditionTrue(context));
	}
}
