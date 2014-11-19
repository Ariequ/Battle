using UnityEngine;
namespace Battle
{
    public class AttackTargetInRangeCondition : Condition
    {                                
        override public bool isConditionTrue(IAIContext context)
        {
            BattleAgent agent = context.Agent as BattleAgent;
            BattleAgent oldTarget = agent.TracingTarget;

            if (oldTarget != null && oldTarget.IsAlive)
            {
				SoldierMeta meta = agent.Metadata;
                float distance = agent.BattleAgentManager.GetDistanceForTarget(agent.Name, oldTarget.Name);

				return distance >= meta.attackMinRange && distance <= meta.attackMaxRange;
            }

            return false;
        }
    }
}
