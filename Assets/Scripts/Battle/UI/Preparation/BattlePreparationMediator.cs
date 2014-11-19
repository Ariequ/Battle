using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class BattlePreparationMediator : AbstractPopupMediator
{
	new public static String NAME = "BattlePreparationMediator";

	private BattlePreparationView view;
	private AlertView alertView;

	private BattleProxy battleProxy;

	private LevelVO currentLevel;

	public BattlePreparationMediator (IPopupManagerDelegate popupManagerDelegate) : base (popupManagerDelegate, NAME, "UI/Battle/Panels/BattlePreparationView")
	{
		battleProxy = ApplicationFacade.Instance.RetrieveProxy(BattleProxy.NAME) as BattleProxy;
	}

	public override IList<string> ListNotificationInterests()
	{
		List<string> notifications = new List<string>();

		notifications.Add(NotificationConst.BATTLE_PREPARATION_SHOW);

		return notifications;
	}

	public override void HandleNotification(INotification notification)
	{
		switch(notification.Name)
		{
		case NotificationConst.BATTLE_PREPARATION_SHOW:

			this.currentLevel = (LevelVO) notification.Body;
			battleProxy.UpdateFromLevelVO(this.currentLevel);

			this.view = CreatePopupView() as BattlePreparationView;

//			GameObject.Find("LordController").GetComponent<LegendCharactorController>().enabled = false;

			break;
		}
	}

	protected override void updateViewShow ()
	{
//		UIEventListener.Get(view.closeButton.gameObject).onClick = OnCloseButtonClick;
		UIEventListener.Get(view.startButton.gameObject).onClick = OnStartButtonClick;
		UIEventListener.Get(view.heroArea.gameObject).onClick = OnHeroAreaClick;
		UIEventListener.Get(view.soldierArea.gameObject).onClick = OnSoldierAreaClick;
//		UIEventListener.Get(view.detectButton.gameObject).onClick = OnLevelDetectClick;
		
		HeroVO[] heroVOArray = battleProxy.GetHeroVOArray(Faction.Self);
		view.UpdateUI(heroVOArray, this.currentLevel);
		
		SendNotification(NotificationConst.UPDATE_BATTLE_FIELD, Faction.Self);
	}

	protected override void updateViewHide ()
	{
		UIEventListener.Get(view.closeButton.gameObject).onClick = null;
		UIEventListener.Get(view.startButton.gameObject).onClick = null;
		UIEventListener.Get(view.heroArea.gameObject).onClick = null;
		UIEventListener.Get(view.soldierArea.gameObject).onClick = null;
		UIEventListener.Get(view.detectButton.gameObject).onClick = null;
	}

	private void OnCloseButtonClick (GameObject go)
	{
		ClosePopupView ();

//		GameObject.Find("LordController").GetComponent<LegendCharactorController>().enabled = true;

	}

	private void OnStartButtonClick (GameObject go)
	{
		UpdateFromLevelVO(battleProxy, MockServer.levelVOList[1]);

		if (battleProxy.ReadyForStart ()) 
		{
			ClosePopupView ();
			
			GameGlobal.LoadSceneByName (SceneNameConst.BATTLE_SCENE);
		} 
		else 
		{
			alertView = AlertView.CreateAndShow("Can Not Start", "Please select Heroes and Soldiers.");
			UIEventListener.Get(alertView.okButton.gameObject).onClick += OnAlertConfirm;
			UIEventListener.Get(alertView.closeButton.gameObject).onClick += OnAlertConfirm;
			view.sceneWindow.Display(false);
		}
	}

	private void UpdateFromLevelVO(BattleProxy battleProxy, LevelVO levelVO)
	{
		LevelMeta levelMeta = levelVO.Meta;
		SoldierVO[] soldierVOList = new SoldierVO[levelMeta.enemySoldierIDs.Length];
		
		for (int i = 0; i < soldierVOList.Length; ++i)
		{
			int soldierID = levelMeta.enemySoldierIDs[i];
			soldierVOList[i] = new SoldierVO(MetaManager.Instance.GetSoldierMeta(soldierID), Faction.Opponent);
		}
		
		battleProxy.SetSoldierVOArray(Faction.Self, soldierVOList);
		
		HeroVO[] heroVOList = new HeroVO[levelMeta.enemyHeroIDs.Length];
		
		for (int i = 0; i < heroVOList.Length; ++i)
		{
			int heroID = levelMeta.enemyHeroIDs[i];
			HeroVO heroVO = new HeroVO(MetaManager.Instance.GetHeroMeta(heroID), Faction.Opponent);
			heroVO.level = levelMeta.enemyHeroLevels[i];
			heroVO.starLevel = levelMeta.enemyHeroStarLevels[i];
			heroVOList[i] = heroVO;
		}
		
		battleProxy.SetHeroVOArray (Faction.Self, heroVOList);
	}

	private void OnHeroAreaClick (GameObject go)
	{
		SendNotification(NotificationConst.CHOOSE_HERO_SHOW, this.currentLevel);
	}

	private void OnSoldierAreaClick (GameObject go)
	{
		SendNotification(NotificationConst.CHOOSE_SOLDIER_SHOW, this.currentLevel);
	}

	private void OnLevelDetectClick (GameObject go)
	{
		ClosePopupView ();
		SendNotification(NotificationConst.OPPONENT_INFO_SHOW, this.currentLevel);
	}

	private void OnAlertConfirm (GameObject go)
	{
		view.sceneWindow.Display(true);
		UIEventListener.Get(alertView.okButton.gameObject).onClick -= OnAlertConfirm;
		UIEventListener.Get(alertView.closeButton.gameObject).onClick -= OnAlertConfirm;
	}
}

