using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class TroopMoveAction : BTAction
    {
        TroopAgent agent;

        override public void onInitialize(IAIContext context)
        {
//            Debug.Log("=======================" + agent.Faction.ToString());

            agent = context.Agent as TroopAgent;
            Vector2 moveTarget = agent.troopMoveTarget;

            if (agent.Faction == Faction.Self)
            {
                Debug.Log("troop move target == " + agent.Name +  moveTarget);
            }


            if (Vector2.IsZero(moveTarget))
            {
                m_eStatus = Status.BH_FAILURE;
                return;
            }
            else
            {
                int index = 0;
//                for (int i=0; i<agent.Soldiers.Values.Count; i++)
//                {
//                    agent.Soldiers.Values[i].TroopPostion = moveTarget + agent.SoliderAnchors[i];
//                }

                foreach (KeyValuePair<string, BattleAgent> kvp in agent.Soldiers)
                {
                    kvp.Value.TroopPostion = moveTarget + agent.SoliderAnchors[index++];
                }

                agent.MoveGroupAbs(MathUtil.ParseToVector3(agent.troopMoveTarget));
                agent.Position = agent.troopMoveTarget;
                m_eStatus = Status.BH_SUCCESS;
            }
        }

//        override public Status update(IAIContext context)
//        {
//            if (m_eStatus == Status.BH_FAILURE)
//            {
//                return Status.BH_FAILURE;
//            }
//
//           // agent.Position = Vector2.LerpTo(agent.Position, agent.troopMoveTarget, agent.speed, BattleTime.deltaTime);// agent.Position + Vector2.Normalize(agent.troopMoveTarget - agent.Position) * BattleTime.deltaTime * agent.speed;
//    
//            agent.MoveGroupAbs(MathUtil.ParseToVector3(agent.troopMoveTarget));
//            agent.Position =
//
//            if (Vector2.Distance(agent.Position, agent.troopMoveTarget) < 1)
//            {
//                return Status.BH_SUCCESS;
//            }
//            else
//            {
//                return Status.BH_RUNNING;
//            }
//        }
    }
}
