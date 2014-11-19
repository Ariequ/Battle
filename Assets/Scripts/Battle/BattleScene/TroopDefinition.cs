using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Battle;

public class TroopDefinition 
{
    public string TOMBSTRONE_PATH = "Battlefields/Tombstone/Tombstone_";

	public string name;

	public SoldierMeta soldlerMeta;
	
    public Faction faction;

	public Battle.Vector2[] soliderAnchors;

	public Battle.Vector2 position;

    public string tombStonePath; 

    public string sideName;
}
