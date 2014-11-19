using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class BattleFieldMediator : Mediator
{
	new public static String NAME = "BattleFieldMediator";

    private const string BATTLE_FIELD_NAME = "MiniScene";

	private BattleFieldView view;
	private BattleProxy battleProxy;

	public BattleFieldMediator () : base (NAME)
	{
        view = GameObject.Find(BATTLE_FIELD_NAME).GetComponent<BattleFieldView>();

		battleProxy = ApplicationFacade.Instance.RetrieveProxy(BattleProxy.NAME) as BattleProxy;
	}

	public override IList<string> ListNotificationInterests()
	{
		List<string> notifications = new List<string>();
		
		notifications.Add(NotificationConst.UPDATE_BATTLE_FIELD);
		
		return notifications;
	}
	
	public override void HandleNotification(INotification notification)
	{
		switch(notification.Name)
		{
    		case NotificationConst.UPDATE_BATTLE_FIELD:
    			UpdateBattleField((Faction)notification.Body);
    			break;
		}
	}

	private void UpdateBattleField(Faction fatcion)
	{
		SoldierVO[] soldierVOArray = battleProxy.GetSoldierVOArray(fatcion);
        LevelVO levelVO = battleProxy.GetCurrentLevel();

		view.UpdateBattleField(fatcion, soldierVOArray, levelVO);
	}
}

