using Battle;

public class AnchorConfigManager 
{
	private static AnchorConfigManager instance;
	
	public static AnchorConfigManager Singleton
	{
		get
		{
			if (instance == null)
			{
				instance = new AnchorConfigManager();
			}
			
			return instance;
		}
	}
	
	private AnchorConfigManager()
	{
	}

	public static Vector2[] largeSoldierAnchors = new Battle.Vector2[]
	{
		new Vector2(0, 0),
	};

	public static Vector2[] mediumSoldierAnchors = new Battle.Vector2[]
	{
		new Vector2(3, -3), new Vector2(3, 3),
		new Vector2(-3, -3), new Vector2(-3, 3),
	};

	public static Vector2[] smallSoldierAnchors = new Battle.Vector2[]
	{
		new Vector2(3, -3), new Vector2(3, 0), new Vector2(3, 3),
		new Vector2(0, -3), new Vector2(0, 0), new Vector2(0, 3),
		new Vector2(-3, -3), new Vector2(-3, 0), new Vector2(-3, 3)
	};

	public static Vector2[] selfTroopAnchors = new Battle.Vector2[]
	{
//		new Vector2(82, 19), new Vector2(89, 28), new Vector2(97, 37), new Vector2(92, 18), new Vector2(99, 27)
		new Vector2(-36, 13), new Vector2(-28, 19), new Vector2(-46, 12), new Vector2(-37, 20), new Vector2(-30, 29)
	};

	public static Vector2[] opponentTroopAnchors = new Battle.Vector2[]
	{
//		new Vector2(44, 73), new Vector2(52, 82), new Vector2(34, 75), new Vector2(43, 83), new Vector2(50, 92)
		new Vector2(2, -44), new Vector2(9, -37), new Vector2(17, -26), new Vector2(12, -45), new Vector2(19, -36)
	};

	public Vector2[] GetSoldierAnchors (SizeLevel sizeLevel)
	{
		switch(sizeLevel)
		{
		case SizeLevel.Large:
			return largeSoldierAnchors;
		case SizeLevel.Medium:
			return mediumSoldierAnchors;
		case SizeLevel.Small:
			return smallSoldierAnchors;
		}

		return null;
	}

	public Vector2 GetTroopAnchor (int index, Faction faction)
	{
		switch(faction)
		{
		case Faction.Self:
			return selfTroopAnchors[index];
		case Faction.Opponent:
			return opponentTroopAnchors[index];
		}

		return Vector2.zero;
	}
}
