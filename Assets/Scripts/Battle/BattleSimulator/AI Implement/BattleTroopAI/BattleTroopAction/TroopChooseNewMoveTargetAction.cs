using System.Collections.Generic;

namespace Battle
{
    public class TroopChooseNewMoveTargetAction : BTAction
    {
        TroopAgent agent;
        BattleAgentManager agentManager;

        override public void onInitialize(IAIContext context)
        {
            agent = context.Agent as TroopAgent;
			Faction enemyFaction = FactionUtil.Revert(agent.Faction);
            agentManager = agent.BattleAgentManager;

            List<TroopAgent> enemyTroops = agentManager.GetTroopsByFaction(enemyFaction);

            if (enemyTroops != null && enemyTroops.Count > 0)
            {
                int checkCount = 0;
                foreach (TroopAgent enemyTroop in enemyTroops)
                {
                    TroopAgent chosenMeTarget = agent.troopChosenMeTarget;

                    if (chosenMeTarget == null || ++checkCount == enemyTroops.Count)
                    {   
                        Vector2 targetTroopPositon = (enemyTroop.Position + 1.0f * agent.Position) / 2.0f;
                        agent.troopChosenMeTarget = enemyTroop;
                        agent.troopMoveTarget = targetTroopPositon;
                        m_eStatus = Status.BH_SUCCESS;
                    }
                }
            }
            else
            {
                m_eStatus = Status.BH_FAILURE;
            }
        }
    }
}
