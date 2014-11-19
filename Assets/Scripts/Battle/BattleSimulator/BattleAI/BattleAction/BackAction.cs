using System.Collections;

namespace Battle
{
    public class BackAction : BTAction
    {   
        float startTime;
        override public void onInitialize(IAIContext context)
        {
            BattleAgent agent = context.Agent as BattleAgent;
            agent.CurrentStatus = BattleAgentStatus.ATTACKED;
            agent.attackStatus = 0;
            startTime = BattleTime.time;
        }

        override public Status update(IAIContext context)
        {   
            if (BattleTime.time - startTime > 1.0f)
            {
                return Status.BH_SUCCESS;
            }
            else
            {
                return Status.BH_RUNNING;
            }
        }
    }
}