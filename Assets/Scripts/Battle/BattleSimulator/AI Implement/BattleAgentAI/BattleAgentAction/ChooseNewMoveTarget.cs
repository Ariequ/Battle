using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class ChooseNewMoveTarget : BTAction
    {
        private float SOLDIER_SEARCH_RADIUS = 10f;
        private UnitController unitInfo;
        private BattleAgent agent;

        override public void onInitialize(IAIContext context)
        {
            agent = context.Agent as BattleAgent;
            unitInfo = agent.UnitController;

            SOLDIER_SEARCH_RADIUS = unitInfo.Meta.attackMaxRange * 2;

            if (agent.TracingTarget == null || !agent.TracingTarget.IsAlive)
            {
                if (Vector2.Distance(agent.Position, agent.TroopPostion) < SOLDIER_SEARCH_RADIUS)
                {
                    if (findTracingTarget())
                    {
                        m_eStatus = Status.BH_SUCCESS;  
                        return;
                    }
                }  
            }

            agent.TracingTarget = null;  
            agent.MoveAgent(agent.TroopPostion);
            m_eStatus = Status.BH_FAILURE;
        }

        private bool findTracingTarget()
        {
            List<BattleAgent> selectedGroup = new List<BattleAgent>();

            Vector3[] selectionPoints = new Vector3[]{
                new Vector3(-SOLDIER_SEARCH_RADIUS,0,-SOLDIER_SEARCH_RADIUS) + MathUtil.ParseToVector3(agent.Position),
                new Vector3(-SOLDIER_SEARCH_RADIUS,0,SOLDIER_SEARCH_RADIUS) + MathUtil.ParseToVector3(agent.Position),
                new Vector3(SOLDIER_SEARCH_RADIUS,0,SOLDIER_SEARCH_RADIUS) + MathUtil.ParseToVector3(agent.Position),
                new Vector3(SOLDIER_SEARCH_RADIUS,0,-SOLDIER_SEARCH_RADIUS) + MathUtil.ParseToVector3(agent.Position)
            };

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
//                if (target.TracingTarget == null || !target.TracingTarget.IsAlive)
//                {
//                    target.TracingTarget = agent;
//                }

                return true;
            }

            return false;         
        }
    }
}