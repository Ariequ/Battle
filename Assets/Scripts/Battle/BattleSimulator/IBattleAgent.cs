namespace Battle
{
    public interface IBattleAgent
    {
		string Name { get; }

        Vector2 Position { get; }

        Vector2 Rotation { get; }

        BattleAgentStatus CurrentStatus { get; }

		SoldierMeta Metadata { get; }

		string AttackTarget { get; }

		int AttackMode { get; }
    }
}

