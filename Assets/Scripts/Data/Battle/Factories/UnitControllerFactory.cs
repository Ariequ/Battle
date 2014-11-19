using UnityEngine;
using System.Collections;

using Battle;

public class UnitControllerFactory 
{
	private static UnitControllerFactory instance;

	public static UnitControllerFactory Singleton
	{
		get
		{
			if (instance == null)
			{
				instance = new UnitControllerFactory();
			}

			return instance;
		}
	}

	private UnitControllerFactory()
	{
	}

	public UnitController CreateUnitController (SoldierMeta soldierMeta, Faction faction, ISkillContainerDelegate skillContainerDelegate)
	{
		SoldierVO soldierVO = new SoldierVO (soldierMeta, faction);
		UnitVO unitVO = new UnitVO (soldierVO);

		return new UnitController (unitVO, skillContainerDelegate);
	}
}
