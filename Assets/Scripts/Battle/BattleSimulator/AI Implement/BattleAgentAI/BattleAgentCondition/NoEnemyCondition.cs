namespace Battle
{
    public class NoEnemyCondition : Condition
    {
        override public bool isConditionTrue(IAIContext context)
        {
            BattleAgent agent = context.Agent as BattleAgent;
			Faction enemyFaction = FactionUtil.Revert(agent.UnitController.Faction);

            return agent.BattleAgentManager.GetTroopsByFaction(enemyFaction).Count < 1;
        }
    }
}
