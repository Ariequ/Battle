using UnityEngine;
using System.Collections;

public class HasTroopConditon : Condition 
{
	GameObject self;
    override public bool isConditionTrue (IAIContext context)
	{
        SideAgent agent = context.Agent as SideAgent;

        return agent.battleAgentManager.GetTroopsByFaction(agent.faction).Count > 0 && agent.battleAgentManager.GetTroopsByFaction(FactionUtil.Revert(agent.faction)).Count > 0;
	}
}
