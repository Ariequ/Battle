namespace Battle
{
    public class NoneSoldierCondition : Condition
    {
     override public bool isConditionTrue(IAIContext context)
        {
            TroopAgent agent = context.Agent as TroopAgent;

            return agent.soldiers.Count < 1;
        }
    }
}
