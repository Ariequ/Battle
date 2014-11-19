using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class WarTempleMediator : UIMediator
{
    new public static String NAME = "WarTempleMediator";

    public const int ID = 1;

    private WarTempleView view;
    private WarTempleProxy proxy;

	private WarTempleMeta meta;

    public WarTempleMediator() : base (NAME)
    {
        proxy = new WarTempleProxy();
        ApplicationFacade.Instance.RegisterProxy(proxy);
    }

    public override void OnRemove()
    {
        ApplicationFacade.Instance.RemoveProxy(WarTempleProxy.NAME);
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
            int id = (int)notification.Body;
			if (id == ID)
			{
				this.meta = proxy.GetMeta(id);
				showView();
			}
            break;
        }
    }

    protected override void showView (object param = null)
    {
		view = popupManagerDelegate.CreateAndAddPopup("UI/Legend/WarTemple/WarTemple") as WarTempleView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.visitButton.gameObject).onClick = onReceiveButtonClick;
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
		UIEventListener.Get(view.visitButton.gameObject).onClick = null;
		UIEventListener.Get(view.closeButton.gameObject).onClick = null;
	}

	private void onCloseButtonClick(GameObject go)
	{
		hideView();
	}

    private void onReceiveButtonClick(GameObject go)
    {
		hideView();
    }
}

