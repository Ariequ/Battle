using System;

using PureMVC.Core;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class ApplicationFacade : Facade
{
	public const string START_UP = "start_up";

	private static ApplicationFacade _instance;

	public static new ApplicationFacade Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (m_staticSyncRoot)
				{
					if (_instance == null) _instance = new ApplicationFacade();
				}
			}
			
			return _instance;
		}
	}

	protected override void InitializeView ()
	{
		base.InitializeView ();
	}

	protected override void InitializeController ()
	{
		base.InitializeController ();

		RegisterCommand(START_UP, typeof(StartUpCommand));
	}

	protected override void InitializeModel ()
	{
		base.InitializeModel ();
	}

	public void StartUp()
	{
		SendNotification(START_UP, this);

		RemoveCommand(START_UP);
	}

    public void RegisterValueProxy(Faction faction)
    {
        RegisterProxy(new BattleValueProxy(faction));
    }

    public BattleValueProxy RetrieveValueProxy(Faction faction)
    {
        return RetrieveProxy(BattleValueProxy.NAME + faction.ToString()) as BattleValueProxy;
    }

    public void RemoveValueProxy(Faction faction)
    {
        RemoveProxy(BattleValueProxy.NAME + faction.ToString());
    }
}


