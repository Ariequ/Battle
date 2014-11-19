using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class HeroUIMediator : UIMediator {

	new public static String NAME = "HeroUIMediator";

	public const string PANEL_PATH = "UI/MainCity/HeroUI/HeroUIPanel";

	private HeroProxy proxy;

	private HeroUIView view;
	

	public HeroUIMediator() : base (NAME)
	{
		proxy = new HeroProxy();
		ApplicationFacade.Instance.RegisterProxy(proxy);
	}

	public override void OnRemove()
	{
		ApplicationFacade.Instance.RemoveProxy(HeroProxy.NAME);
	}

	public override IList<string> ListNotificationInterests()
	{
		List<string> notifications = new List<string>();
		
		notifications.Add(NotificationConst.HEROUI_MAIN_SHOW_PANEL);
		
		return notifications;
	}

	public override void HandleNotification(INotification notification)
	{
		view = popupManagerDelegate.CreatePopupCanvas(PANEL_PATH) as HeroUIView;
		initView();
		showView();
	}

	protected override void showView (object param = null)
	{

		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
	}

	private void initView ()
	{
		view.closeButton.onClick.AddListener(onCloseButtonClick);
		HeroVO[] HeroVO = proxy.GetHeroVOArray();
		view.InitUI (HeroVO);
		foreach (GameObject card in view.heroCardList) 
		{
			card.transform.GetComponent<Button>().onClick.AddListener(onHeroCardButtonClick);
		}
	}

	protected override void updateViewShow ()
	{
	}
	
	protected override void hideView (object param = null)
	{
		popupManagerDelegate.RemovePopup(view,true);
		view.UpdateShowDelegate = null;
		view.UpdateHideDelegate = null;
	}
	
	protected override void updateViewHide ()
	{
		//view.closeButton.onClick.RemoveAllListeners ();
	}
	
	private void onCloseButtonClick()
	{
		hideView();
	}

	public void onHeroCardButtonClick()
	{
		SendNotification(NotificationConst.HEROUI_DEARAILS_SHOW_PANEL);
	}
}
