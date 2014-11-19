namespace Battle
{
    public class TroopDieAction : BTAction
    {
        override public void onInitialize(IAIContext context)
        {
            TroopAgent agent = context.Agent as TroopAgent;
            agent.CurrentStatus = BattleAgentStatus.DIE;
            m_eStatus = Status.BH_SUCCESS;   
        }
    }
}
