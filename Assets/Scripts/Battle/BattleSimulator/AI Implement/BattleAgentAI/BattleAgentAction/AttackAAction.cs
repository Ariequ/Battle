using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Battle
{
    public class AttackAAction : BTAction
    {
        float startTime;
        UnitController unitVO;
        BattleAgent agent;

        override public void onInitialize(IAIContext context)
        {
            agent = context.Agent as BattleAgent;
            BattleAgent attackTarget = agent.TracingTarget;

            if (attackTarget== null || !attackTarget.IsAlive)
            {
                m_eStatus = Status.BH_FAILURE;
            }
            else
            {
                unitVO = agent.UnitController;
                agent.BattleAgentManager.DoAttack(agent, attackTarget, unitVO.Meta.bulletMeta);
                
                agent.Rotation = attackTarget.Position - agent.Position;
                startTime = BattleTime.time;
                agent.CurrentStatus = BattleAgentStatus.ATTACK;
                
                agent.IsEnabled = false;
             }
        }
    
        override public Status update(IAIContext context)
        { 
            if (m_eStatus == Status.BH_FAILURE)
            {
                agent.IsEnabled = true;
                return Status.BH_FAILURE;
            }
            else if (BattleTime.time - startTime > unitVO.Meta.attackFrequency)
            {
                agent.IsEnabled = true;
                return Status.BH_SUCCESS;
            }
            else
            {
                return Status.BH_RUNNING;
            }
        }
    }
}
