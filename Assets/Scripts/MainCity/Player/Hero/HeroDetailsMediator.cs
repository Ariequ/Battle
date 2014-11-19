﻿using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class HeroDetailsMediator : UIMediator {
	
	new public static String NAME = "HeroDetailsMediator";
	
	public const string PANEL_PATH = "UI/MainCity/HeroUI/HeroDetails";

	private HeroProxy proxy;
	
	private HeroDetailsView view;

	private HeroVO heroVO;
	
	public HeroDetailsMediator() : base (NAME)
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
		
		notifications.Add(NotificationConst.HEROUI_DEARAILS_SHOW_PANEL);
		
		return notifications;
	}

	public override void HandleNotification(INotification notification)
	{
		showView();
		heroVO = ButtonOnClick.buttonobj.GetComponent<HeroCard> ().heroVO;
		Debug.Log (heroVO.Attack);

	}

	protected override void showView (object param = null)
	{
		view = popupManagerDelegate.CreatePopupCanvas(PANEL_PATH, PopupMode.DEFAULT, PopupQueueMode.QueueFrontShow) as HeroDetailsView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
	}

	protected override void updateViewShow ()
	{
		view.closeButton.onClick.AddListener(onCloseButtonClick);
	}

	protected override void hideView (object param = null)
	{
		popupManagerDelegate.RemovePopup(view,true);
		view.UpdateShowDelegate = null;
		view.UpdateHideDelegate = null;
	}

	protected override void updateViewHide ()
	{
		view.closeButton.onClick.RemoveAllListeners ();
	}
	
	private void onCloseButtonClick()
	{
		hideView();
	}
}
