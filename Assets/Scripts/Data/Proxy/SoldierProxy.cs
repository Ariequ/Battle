using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;

public class SoldierProxy : Proxy
{
	new public const string NAME = "SoldierProxy";

	public SoldierProxy () : base(NAME)
	{

	}
	
	public override void OnRemove ()
	{
	
	}

	public SoldierVO[] GetPlayerSoldierList()
	{
		return MockServer.soldierVOList.ToArray();
	}
}
