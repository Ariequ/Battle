using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBehaviorCondition 
{
    bool isConditionTrue(IAIContext context);
	void setParam(Dictionary<string,string> param);
	Dictionary<string,string> getParam();
}
