using System.Collections;

namespace Battle
{
    public class IdelAction : BTAction
    {
        private float idelTime = 2.0f;

        override public void onInitialize(IAIContext context)
        {
            BattleAgent agent = context.Agent as BattleAgent;
            agent.CurrentStatus = BattleAgentStatus.IDLE;
            agent.IsEnabled = false;
        }

        override public Status update(IAIContext context)
        {
			idelTime -= BattleSimulator.deltaTime;

            if (idelTime < 0)
            {
                return Status.BH_SUCCESS;
            }
            else
            {
                return Status.BH_RUNNING;
            }
        }

        override public void onTerminate(Status status)
        {
            idelTime = 2.0f;
        }
    }
}