using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class PrisonMediator : UIMediator
{
    new public static String NAME = "PrisonMediator";

    public const int ID = 6;

    private PrisonView view;
    private PrisonProxy proxy;
	private PrisonMeta meta;

    public PrisonMediator() : base (NAME)
    {
        proxy = new PrisonProxy();
        ApplicationFacade.Instance.RegisterProxy(proxy);
    }

    public override void OnRemove()
    {
        ApplicationFacade.Instance.RemoveProxy(PrisonProxy.NAME);
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
		view = popupManagerDelegate.CreateAndAddPopup("UI/Legend/Prison/Prison") as PrisonView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.confirmButton.gameObject).onClick = onConfirmButtonClick;
		UIEventListener.Get(view.refuseButton.gameObject).onClick = onRefuseButtonClick;
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
		UIEventListener.Get(view.confirmButton.gameObject).onClick = null;
		UIEventListener.Get(view.refuseButton.gameObject).onClick = null;
	}

    private void onConfirmButtonClick(GameObject go)
    {
		hideView();
    }

    private void onRefuseButtonClick(GameObject go)
    {
		hideView();
    }
}

