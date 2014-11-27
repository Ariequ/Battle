using System.Collections;
using UnityEngine;

namespace Battle
{
    public class DieAction : BTAction
    {
        override public void onInitialize(IAIContext context)
        {
            BattleAgent agent = context.Agent as BattleAgent;
            agent.CurrentStatus = BattleAgentStatus.DIE;
            agent.IsEnabled = false;
            m_eStatus = Status.BH_SUCCESS;
        }
    }
}
