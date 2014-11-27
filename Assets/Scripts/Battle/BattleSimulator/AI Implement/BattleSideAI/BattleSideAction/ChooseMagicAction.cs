using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseMagicAction : BTAction
{
    override public void onInitialize (IAIContext context)
	{
        SideAgent agent = context.Agent as SideAgent;

		if (RandomUtil.Range(1,1000) < 5)
        {
            agent.CurrentSkillMeta = agent.skillMetaList[RandomUtil.Range(0, agent.skillMetaList.Count)];

            m_eStatus = Status.BH_SUCCESS;
        }
        else
        {
            m_eStatus = Status.BH_FAILURE;
        }
	}
}

