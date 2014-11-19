using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class MainInterfaceMediator : UIMediator
{
	public new const string NAME = "MainInterfaceMediator";

	private MainInterfaceView view;

	public MainInterfaceMediator ():base(NAME)
	{
	}

	public override IList<string> ListNotificationInterests ()
	{
		List<string> notifications = new List<string> ();
		
		notifications.Add (NotificationConst.MAIN_INTERFACE_SHOW);
		
		return notifications;
	}
	
	public override void HandleNotification (INotification notification)
	{
		switch (notification.Name)
		{
		case NotificationConst.MAIN_INTERFACE_SHOW:
			showView ();
			break;
		}
	}

    protected override void showView (object param = null)
	{
		view = popupManagerDelegate.CreateAndAddPopup("MainInterface/MainInterface") as MainInterfaceView;

        UIEventListener.Get(view.worldMapButton.gameObject).onClick = onWorldMapButtonClick;
        UIEventListener.Get(view.mainCityButton.gameObject).onClick = onMainCityButtonClick;
    }

	protected override void updateViewShow ()
	{
	}

	protected override void hideView (object param = null)
	{
		popupManagerDelegate.RemovePopup(view, true);
	}

	protected override void updateViewHide ()
	{
	}

    private void onWorldMapButtonClick(GameObject go)
    {
		hideView();
//		GameGlobal.LoadSceneByName (SceneNameConst.LEGEND_SCENE_);

		MockServer.worldVO.inBattleMonsterID = 1;
		LevelProxy levelProxy = ApplicationFacade.Instance.RetrieveProxy(LevelProxy.NAME) as LevelProxy;
		ApplicationFacade.Instance.SendNotification (NotificationConst.BATTLE_PREPARATION_SHOW, levelProxy.GetLevelVO(1));
    }

    private void onMainCityButtonClick(GameObject go)
    {
		hideView();
        GameGlobal.LoadSceneByName(SceneNameConst.MAIN_CITY);
    }
    
}
