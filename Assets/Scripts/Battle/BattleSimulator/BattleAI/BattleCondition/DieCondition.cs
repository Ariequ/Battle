using UnityEngine;

namespace Battle
{
    public class DieCondition : Condition
    {
        override public bool isConditionTrue(IAIContext context)
        {
            return (context.Agent as BattleAgent).UnitController.HP <= 0;
        }
    }
}
