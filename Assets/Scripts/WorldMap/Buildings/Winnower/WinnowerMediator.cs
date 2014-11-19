using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class WinnowerMediator : UIMediator
{
    new public static String NAME = "WinnowerMediator";

    public const int ID = 7;

    private WinnowerView view;
    private WinnowerProxy proxy;

	private WinnowerMeta meta;

    public WinnowerMediator() : base (NAME)
    {
        proxy = new WinnowerProxy();
        ApplicationFacade.Instance.RegisterProxy(proxy);
    }

    public override void OnRemove()
    {
        ApplicationFacade.Instance.RemoveProxy(WinnowerProxy.NAME);
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();

        notifications.Add(NotificationConst.EXPLORE_BUILDING_SHOW);

        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
        case NotificationConst.EXPLORE_BUILDING_SHOW:
			if ((int)notification.Body == ID)
			{
				meta = proxy.GetMeta();
				showView();
			}
            break;
        }
    }

    protected override void showView (object param = null)
    {
		view = popupManagerDelegate.CreateAndAddPopup("UI/Legend/Winnower/Winnower") as WinnowerView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.receiveButton.gameObject).onClick = onReceiveButtonClick;
		UIEventListener.Get(view.closeButton.gameObject).onClick = onCloseButtonClick;
		view.UpdateUI(meta);
	}

	protected override void hideView (object param = null)
    {
		popupManagerDelegate.RemovePopup(view);
		view.UpdateShowDelegate = null;
		view.UpdateHideDelegate = null;
    }

	protected override void updateViewHide ()
	{
		UIEventListener.Get(view.receiveButton.gameObject).onClick = null;
		UIEventListener.Get(view.closeButton.gameObject).onClick = null;
	}

    private void onReceiveButtonClick(GameObject go)
    {
		hideView();
    }

	private void onCloseButtonClick(GameObject go)
	{
		hideView();
	}
}

