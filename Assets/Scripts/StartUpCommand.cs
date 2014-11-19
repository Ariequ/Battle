using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Interfaces;
using PureMVC.Patterns;

public class StartUpCommand : SimpleCommand
{
	public override void Execute (INotification notification)
	{
		InitializePureMVC();
	}

	void InitializePureMVC ()
	{
		ApplicationFacade facade = ApplicationFacade.Instance;

		facade.RegisterProxy(new HeroProxy());
		facade.RegisterProxy(new SoldierProxy());
		facade.RegisterProxy(new LevelProxy());
		facade.RegisterProxy(new BattleProxy());
	}
}

