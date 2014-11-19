using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class ChooseHeroMediator : UIMediator
{
	new public static String NAME = "ChooseHeroMediator";

	private ChooseHeroView view;

	private BattleProxy battleProxy;
	private HeroProxy heroProxy;

	private LevelVO currentLevel;

	public ChooseHeroMediator () : base (NAME)
	{
		battleProxy = ApplicationFacade.Instance.RetrieveProxy(BattleProxy.NAME) as BattleProxy;
		heroProxy = ApplicationFacade.Instance.RetrieveProxy(HeroProxy.NAME) as HeroProxy;
	}

	public override IList<string> ListNotificationInterests()
	{
		List<string> notifications = new List<string>();

		notifications.Add(NotificationConst.CHOOSE_HERO_SHOW);
		
		return notifications;
	}

	public override void HandleNotification(INotification notification)
	{
		switch(notification.Name)
		{
		case NotificationConst.CHOOSE_HERO_SHOW:

			this.currentLevel = (LevelVO) notification.Body;
			showView();
			break;
		}
	}

    protected override void showView (object param = null)
	{
		view = popupManagerDelegate.CreateAndAddPopup("UI/Battle/Panels/BattleChooseHeroView", PopupMode.DEFAULT, PopupQueueMode.QueueFrontShow) as ChooseHeroView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;

//		GameObject.Find("LordController").GetComponent<LegendCharactorController>().enabled = false;

	}

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.recruitButton.gameObject).onClick = OnRecruitButtonClick;
		UIEventListener.Get(view.returnButton.gameObject).onClick = OnReturnButtonClick;
		
		HeroVO[] allHeroVOArray = heroProxy.GetHeroVOArray();
		HeroVO[] battleHeroes = battleProxy.GetHeroVOArray(Faction.Self);
		
		view.UpdateUI(allHeroVOArray, battleHeroes);
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
		UIEventListener.Get(view.recruitButton.gameObject).onClick = null;
		UIEventListener.Get(view.returnButton.gameObject).onClick = null;
	}

	private void OnRecruitButtonClick (GameObject go)
	{
		Debug.Log("recruit");
	}

	private void OnReturnButtonClick (GameObject go)
	{
		battleProxy.SetHeroVOArray(Faction.Self, view.GetBattleHeroes());

		hideView();
	}
}

