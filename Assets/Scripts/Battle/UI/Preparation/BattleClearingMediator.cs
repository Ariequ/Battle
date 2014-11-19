using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class BattleClearingMediator : UIMediator 
{
	new public static string NAME = "BattleClearingMediator";
	
	private BattleClearingView view;

	private BattleProxy _battleProxy;

	private bool battleVictory;

	public BattleClearingMediator () : base (NAME)
	{
		this._battleProxy = ApplicationFacade.Instance.RetrieveProxy(BattleProxy.NAME) as BattleProxy;
	}

	public override IList<string> ListNotificationInterests()
	{
		List<string> notifications = new List<string>();
		
		notifications.Add(NotificationConst.BATTLE_COMPLETE);
		
		return notifications;
	}
	
	public override void HandleNotification(INotification notification)
	{
		switch(notification.Name)
		{
		case NotificationConst.BATTLE_COMPLETE:
			this.battleVictory = (bool) notification.Body;
			showView();
			break;
		}
	}

    protected override void showView (object param = null)
	{
		view = popupManagerDelegate.CreateAndAddPopup("UI/Battle/Panels/BattleClearingView") as BattleClearingView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
	}

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.closeButton.gameObject).onClick = OnCloseButtonClick;
		
		view.UpdateUI (_battleProxy.GetCurrentLevel(), this.battleVictory);
	}
	
	protected override void hideView (object param = null)
	{
		popupManagerDelegate.RemovePopup(view);
		view.UpdateShowDelegate = null;
		view.UpdateHideDelegate = null;
	}

	protected override void updateViewHide ()
	{
		UIEventListener.Get(view.closeButton.gameObject).onClick = null;
	}

	private void OnCloseButtonClick (GameObject button)
	{
		view.DisableCloseButton ();
//		GameGlobal.LoadSceneByName (SceneNameConst.LEGEND_SCENE_ + GameGlobal.lastLegendSceneName);

		GameGlobal.LoadSceneByName (SceneNameConst.MAIN_INTERFACE);
	}
}
