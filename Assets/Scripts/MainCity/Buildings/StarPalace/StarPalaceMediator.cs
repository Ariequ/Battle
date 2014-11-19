using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class StarPalaceMediator : UIMediator
{
    new public static String NAME = "StarPalaceMediator";

    private StarPalaceView view;
    private StarPalaceProxy proxy;

	private int tax;

    public StarPalaceMediator() : base (NAME)
    {
        proxy = new StarPalaceProxy();
        ApplicationFacade.Instance.RegisterProxy(proxy);
    }

    public override void OnRemove()
    {
        ApplicationFacade.Instance.RemoveProxy(StarPalaceProxy.NAME);
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();

        notifications.Add(NotificationConst.STARPALACE_SHOW);

        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case NotificationConst.STARPALACE_SHOW:
                showView(1);
                proxy.GetData();
                break;
            case NotificationConst.STARPALACE_UPDATE:
                tax = (int)(notification.Body);
                showView(tax);
                break;
        }
    }

    protected override void showView (object param = null)
    {
    	view = (StarPalaceView)popupManagerDelegate.CreateAndAddPopup("UI/MainCity/StartPalaceView");
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.closeButton.gameObject).onClick = onCloseButtonClick;
		UIEventListener.Get(view.worshipButton.gameObject).onClick = onWorshipButtonClick;
		
		view.InitializeUI(tax);
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

    private void onCloseButtonClick(GameObject go)
    {
        hideView();
    }

    private void onWorshipButtonClick(GameObject go)
    {
        GodnessMeta meta1 = proxy.GetMeta();
        GodnessMeta meta2 = proxy.GetMeta();

        view.UpdateUI(meta1, meta2);

    }
}

