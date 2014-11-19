using UnityEngine;
using System.Collections;

public class FactionUtil 
{
	public static string ParseToTag (Faction faction)
	{
		switch(faction)
		{
		case Faction.Opponent:
			return Tags.ENEMY;
		case Faction.Self:
			return Tags.PLAYER;
		default:
			return null;
		}
	}

	public static Faction Revert (Faction faction)
	{
		switch(faction)
		{
		case Faction.Opponent:
			return Faction.Self;
		case Faction.Self:
			return Faction.Opponent;
		default:
			return Faction.All;
		}
	}
}
