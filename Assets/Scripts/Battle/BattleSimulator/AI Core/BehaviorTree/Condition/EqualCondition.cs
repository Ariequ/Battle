using UnityEngine;
using System.Collections;

public class EqualCondition : Condition
{
    override public bool isConditionTrue (IAIContext context)
	{
        return true;
	}
}
