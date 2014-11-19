using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Condition:IBehaviorCondition
{
	private Dictionary<string,string> m_Param;

    public virtual bool isConditionTrue (IAIContext context)
	{
		return false;
	}

	virtual public void setParam (Dictionary<string,string> param)
	{
		m_Param = param;
	}

	virtual public Dictionary<string,string> getParam ()
	{
		return m_Param;
	}
}
