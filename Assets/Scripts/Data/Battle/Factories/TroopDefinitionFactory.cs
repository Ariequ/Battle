using UnityEngine;
using System.Collections;

public class TroopDefinitionFactory 
{
	private static TroopDefinitionFactory instance;
	
	public static TroopDefinitionFactory Singleton
	{
		get
		{
			if (instance == null)
			{
				instance = new TroopDefinitionFactory();
			}
			
			return instance;
		}
	}
	
	private TroopDefinitionFactory()
	{
	}

	public TroopDefinition CreateTroopDefinition(int index, SoldierMeta soldlerMeta, Faction faction)
	{
		TroopDefinition troopDefinition = new TroopDefinition ();
		troopDefinition.name = index + "_" + faction.ToString();
		troopDefinition.soldlerMeta = soldlerMeta;
		troopDefinition.faction = faction;
		troopDefinition.soliderAnchors = AnchorConfigManager.Singleton.GetSoldierAnchors (soldlerMeta.sizeLevel);
		troopDefinition.position = AnchorConfigManager.Singleton.GetTroopAnchor (index, faction);
        troopDefinition.sideName = faction.ToString();
        troopDefinition.tombStonePath = troopDefinition.TOMBSTRONE_PATH + soldlerMeta.sizeLevel.ToString();

		return troopDefinition;
	}
}
