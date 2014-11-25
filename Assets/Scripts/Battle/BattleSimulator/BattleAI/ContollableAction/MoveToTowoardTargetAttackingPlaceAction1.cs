using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class MoveToTowoardTargetAttackingPlaceAction1 : BTAction
    {
        private float SOLDIER_SEARCH_RADIUS;
        private BattleAgent agent;  
        private float tracingLastTime;
        private int checkSuccessFrames;
        
        override public void onInitialize(IAIContext context)
        {
            agent = context.Agent as BattleAgent;
            SOLDIER_SEARCH_RADIUS = agent.UnitController.Meta.attackMaxRange + agent.UnitController.Meta.boundsRadius;
            agent.CurrentStatus = BattleAgentStatus.MOVE;
            tracingLastTime = 10f;
            agent.MoveAgent(agent.TroopPostion);
            checkSuccessFrames = 100;
        }
        
        override public Status update(IAIContext context)
        {        
            if (!agent.IsAlive || agent.TracingTarget == null || !agent.TracingTarget.IsAlive)
            {
                agent.TracingTarget = null;
                return Status.BH_FAILURE;
            }
            else if (checkSuccessFrames < 0 && (tracingTargetInAttackRange()))
            {
                return Status.BH_SUCCESS;
            }
            else
            {  
                if (checkSuccessFrames-- < 0)
                {
                    checkSuccessFrames = 100;
                }

                if (tracingLastTime < 0)
                { 
                    agent.TracingTarget = null;
                    return Status.BH_FAILURE;
                }
                else
                {
					tracingLastTime -= BattleSimulator.deltaTime;
                    agent.MoveAgent(agent.TracingTarget.Position);
                }

                return Status.BH_RUNNING;
            }
        }

        private bool hasTargetInAttackRange()
        {
            List<BattleAgent> selectedGroup = new List<BattleAgent>();
            Vector3[] selectionPoints = new Vector3[]{
                new Vector3(-SOLDIER_SEARCH_RADIUS,0,-SOLDIER_SEARCH_RADIUS) + MathUtil.ParseToVector3(agent.Position),
                new Vector3(-SOLDIER_SEARCH_RADIUS,0,SOLDIER_SEARCH_RADIUS) + MathUtil.ParseToVector3(agent.Position),
                new Vector3(SOLDIER_SEARCH_RADIUS,0,SOLDIER_SEARCH_RADIUS) + MathUtil.ParseToVector3(agent.Position),
                new Vector3(SOLDIER_SEARCH_RADIUS,0,-SOLDIER_SEARCH_RADIUS) + MathUtil.ParseToVector3(agent.Position)
            };

//            Debug.DrawLine(selectionPoints[0], selectionPoints[1]);
//            Debug.DrawLine(selectionPoints[2], selectionPoints[1]);
//            Debug.DrawLine(selectionPoints[2], selectionPoints[3]);
//            Debug.DrawLine(selectionPoints[0], selectionPoints[3]);

            int[] selectedAgents = FameManager.QueryAgents(selectionPoints);

            foreach (int i in selectedAgents)
            {
                BattleAgent unit = FameManager.GetFlockMember(i);
                
                if (unit.UnitController.Faction != agent.UnitController.Faction)
                {
                    selectedGroup.Add(unit);
                }
            }
            
            if (selectedGroup.Count > 0)
            {
				BattleAgent target = selectedGroup[RandomUtil.Range(0, selectedGroup.Count)];
                agent.TracingTarget = target;

                return true;
            }
            
            return false;         
        }

        private bool tracingTargetInAttackRange()
        {
            if (agent.TracingTarget != null && agent.TracingTarget.IsAlive)
            {
                SoldierMeta meta = agent.Metadata;
                float distance = Vector2.Distance(agent.Position, agent.TracingTarget.Position) - agent.Metadata.boundsRadius - agent.Metadata.boundsRadius;    
                return distance >= meta.attackMinRange && distance <= meta.attackMaxRange;
            }

            return false;
        }

    }
}
