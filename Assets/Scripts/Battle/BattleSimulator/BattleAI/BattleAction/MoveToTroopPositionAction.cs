using UnityEngine;
namespace Battle
{
    public class MoveToTroopPositionAction : BTAction
    {
        private BattleAgent agent;
        private float actionLastTime;

        override public void onInitialize(IAIContext context)
        {
            agent = context as BattleAgent;
            actionLastTime = 0.1f;
            agent.MoveAgent(agent.TroopPostion);
        }
    
        override public Status update(IAIContext context)
        {
            if (actionLastTime < 0)// || agent.ReachGoal())
            {
                return Status.BH_SUCCESS;
            }
            else
            {
//                agent.MoveAndRotate();  
				actionLastTime -= BattleSimulator.deltaTime;
                return Status.BH_RUNNING;
            }
        }
    }
}