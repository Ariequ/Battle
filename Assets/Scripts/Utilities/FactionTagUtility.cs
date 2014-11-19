using System;

public class FactionTagUtility
{
	public static string GetTag(Faction faction)
	{
		return faction == Faction.Self ? Tags.PLAYER : Tags.ENEMY;
	}
	
	public static Faction GetFaction(string tag)
	{
		return tag == Tags.PLAYER ? Faction.Self : Faction.Opponent;
	}

	public static string GetOppositeTag(string tag)
	{
		return tag == Tags.PLAYER ? Tags.ENEMY : Tags.PLAYER;
	}

	public static Faction GetOppositeFaction(Faction faction)
	{
		return faction == Faction.Self ?  Faction.Opponent : Faction.Self;
	}

}

