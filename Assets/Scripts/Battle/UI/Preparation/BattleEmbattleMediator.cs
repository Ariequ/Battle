using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class BattleEmbattleMediator : UIMediator {

	new public static String NAME = "BattleEmbattleMediator";

	private BattleEmbattleView view;
	
	private BattleProxy battleProxy;

	private SoldierProxy soldierProxy;

	private LevelVO currentLevel;

	public BattleEmbattleMediator () : base (NAME)
	{
		this.battleProxy = ApplicationFacade.Instance.RetrieveProxy(BattleProxy.NAME) as BattleProxy;
		this.soldierProxy = ApplicationFacade.Instance.RetrieveProxy(SoldierProxy.NAME) as SoldierProxy;
	}

	public override IList<string> ListNotificationInterests()
	{
		List<string> notifications = new List<string>();
		notifications.Add(NotificationConst.CHOOSE_SOLDIER_SHOW);
		
		return notifications;
	}
	
	public override void HandleNotification(INotification notification)
	{
		switch(notification.Name)
		{
		case NotificationConst.CHOOSE_SOLDIER_SHOW:
			this.currentLevel = (LevelVO) notification.Body;
			showView();
			break;
		}
	}

    protected override void showView (object param = null)
	{
		view = popupManagerDelegate.CreateAndAddPopup("UI/Battle/Panels/BattleEmbattleView", PopupMode.DEFAULT, PopupQueueMode.QueueFrontShow) as BattleEmbattleView;	
		view.UpdateShowDelegate = this.updateViewShow;
		view.UpdateHideDelegate = this.updateViewHide;
//		GameObject.Find("LordController").GetComponent<LegendCharactorController>().enabled = false;
	}

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.confirmButton.gameObject).onClick = OnConfirmButtonClick;
		
		SoldierVO[] allSoldiers = soldierProxy.GetPlayerSoldierList();
		SoldierVO[] battleSoldiers = battleProxy.GetSoldierVOArray(Faction.Self);
		
		const int UNLOCKED_ANCHOR = 2;
		
		view.UpdateUI(allSoldiers, battleSoldiers, UNLOCKED_ANCHOR);
	}

	protected override void hideView (object param = null)
	{
		popupManagerDelegate.RemovePopup(view);
		view.UpdateShowDelegate = null;
		view.UpdateHideDelegate = null;

//		GameObject.Find("LordController").GetComponent<LegendCharactorController>().enabled = true;
	}

	protected override void updateViewHide ()
	{
		UIEventListener.Get(view.confirmButton.gameObject).onClick = null;
	}

	private void OnConfirmButtonClick (GameObject button)
	{
		battleProxy.SetSoldierVOArray(Faction.Self, view.GetBattleSoldiers());

		hideView();
	}
}
