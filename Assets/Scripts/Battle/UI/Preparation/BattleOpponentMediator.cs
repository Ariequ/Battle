using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class BattleOpponentMediator : UIMediator
{
	new public static String NAME = "BattleOpponentMediator";
	
	private BattleOpponentView view;
	private AlertView alertView;

	private BattleProxy battleProxy;
	
	private LevelVO currentLevel;
	
	public BattleOpponentMediator () : base (NAME)
	{
		battleProxy = ApplicationFacade.Instance.RetrieveProxy(BattleProxy.NAME) as BattleProxy;
	}
	
	public override IList<string> ListNotificationInterests()
	{
		List<string> notifications = new List<string>();
		
		notifications.Add(NotificationConst.OPPONENT_INFO_SHOW);
		
		return notifications;
	}
	
	public override void HandleNotification(INotification notification)
	{
		switch(notification.Name)
		{
		case NotificationConst.OPPONENT_INFO_SHOW:
			this.currentLevel = (LevelVO)notification.Body;
			showView();
			break;
		}
	}
	
    protected override void showView (object param = null)
	{
		view = popupManagerDelegate.CreateAndAddPopup("UI/Battle/Panels/BattleOpponentView") as BattleOpponentView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
	}

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.closeButton.gameObject).onClick = OnCloseButtonClick;
		UIEventListener.Get(view.startButton.gameObject).onClick = OnStartButtonClick;
		UIEventListener.Get(view.returnButton.gameObject).onClick = OnReturnButtonClick;
		
		HeroVO[] heros = battleProxy.GetHeroVOArray (Faction.Opponent);
		SoldierVO[] soldiers = battleProxy.GetSoldierVOArray (Faction.Opponent);
		SkillMeta[] skillMetas = new SkillMeta[heros.Length];
		SoldierMeta[] soldierMetas = new SoldierMeta[soldiers.Length];
		
		for (int i = 0; i < heros.Length; i++)
		{
			HeroVO heroVO = heros[i];
			
			if (heroVO != null)
			{
				skillMetas[i] = MetaManager.Instance.GetSkillMeta(heroVO.Meta.skillDic[heroVO.starLevel]);
			}
		}
		
		for (int i = 0; i < soldiers.Length; i++)
		{
			soldierMetas[i] = soldiers[i] != null ? soldiers[i].Meta : null;
		}
		
		view.UpdateUI(skillMetas, soldierMetas, currentLevel.Meta.description);
		
		SendNotification(NotificationConst.UPDATE_BATTLE_FIELD, Faction.Opponent);
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
		UIEventListener.Get(view.closeButton.gameObject).onClick = null;
		UIEventListener.Get(view.startButton.gameObject).onClick = null;
		UIEventListener.Get(view.returnButton.gameObject).onClick = null;
	}

	private void OnCloseButtonClick (GameObject go)
	{
		hideView();
	}
	
	private void OnStartButtonClick (GameObject go)
	{
		if (battleProxy.ReadyForStart ()) 
		{
			hideView();

			GameGlobal.LoadSceneByName (SceneNameConst.BATTLE_SCENE);
		} 
		else 
		{
			alertView = AlertView.CreateAndShow("Can Not Start", "Please select Heroes and Soldiers.");
			UIEventListener.Get(alertView.okButton.gameObject).onClick += OnAlertConfirm;
			UIEventListener.Get(alertView.closeButton.gameObject).onClick += OnAlertConfirm;
		}
	}
	
	private void OnReturnButtonClick (GameObject go)
	{
		hideView();
		SendNotification(NotificationConst.BATTLE_PREPARATION_SHOW, this.currentLevel);
	}

	private void OnAlertConfirm (GameObject go)
	{
		view.sceneWindow.Display(true);
		UIEventListener.Get(alertView.okButton.gameObject).onClick -= OnAlertConfirm;
		UIEventListener.Get(alertView.closeButton.gameObject).onClick -= OnAlertConfirm;
	}
}