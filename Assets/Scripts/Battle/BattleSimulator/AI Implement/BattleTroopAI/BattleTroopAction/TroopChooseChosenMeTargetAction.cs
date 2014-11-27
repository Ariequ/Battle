namespace Battle
{
    public class TroopChooseChosenMeTargetAction : BTAction
    {
        private TroopAgent chosenMeEnemyTroop;
        private TroopAgent agent;
    
        override public void onInitialize(IAIContext context)
        {
            agent = context.Agent as TroopAgent;
            chosenMeEnemyTroop = agent.troopChosenMeTarget;

            if (chosenMeEnemyTroop != null)
            {           
                Vector2 myPosition = agent.Position;
                Vector2 targetTroopPositon = (chosenMeEnemyTroop.Position + 1.0f * agent.Position) / 2.0f;

                chosenMeEnemyTroop.troopChosenMeTarget = agent;

                if (Vector2.Distance(myPosition, targetTroopPositon) < 10)
                {
                    agent.troopMoveTarget = Vector2.zero;
                    m_eStatus = Status.BH_SUCCESS;
                    return;
                }
                else
                {
                    agent.troopMoveTarget = targetTroopPositon;
                    m_eStatus = Status.BH_SUCCESS;
                }
            }
            else
            {
                m_eStatus = Status.BH_FAILURE;
            }
        }
    }
}
