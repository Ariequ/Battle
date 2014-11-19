using System;
using System.Collections;

using Battle;

public class LevelMeta
{
	public int id;

	public string name;

	public string description;
	
	public Vector2 selfAnchor;
	public Vector2 opponentAnchor;

	public Vector2[] selfPositions;
	public Vector2[] opponentPositions;
	
	public int[] enemySoldierIDs;
	public int[] enemySoldierLevels;

	public int[] enemyHeroIDs;
	public int[] enemyHeroLevels;
	public int[] enemyHeroStarLevels;

	public string battlefieldID;
	public int battlefieldSize;
}

