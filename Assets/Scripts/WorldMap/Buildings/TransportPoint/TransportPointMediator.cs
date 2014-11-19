using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class TransportPointMediator : UIMediator
{
    new public static String NAME = "TransferPositionMediator";

    private TransportPointView view;
    private TransportPointProxy proxy;
	private TransportPointMeta meta;

    public TransportPointMediator() : base (NAME)
    {
        proxy = new TransportPointProxy();
        ApplicationFacade.Instance.RegisterProxy(proxy);

        if (!ApplicationFacade.Instance.HasMediator(TransportPointDetailMediator.NAME))
        {
            ApplicationFacade.Instance.RegisterMediator(new TransportPointDetailMediator());
        }
    }

    public override void OnRemove()
    {
        ApplicationFacade.Instance.RemoveProxy(TransportPointProxy.NAME);
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();

        notifications.Add(NotificationConst.TRANSPORTPOINT_SHOW);

        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
        case NotificationConst.TRANSPORTPOINT_SHOW:
			meta = proxy.GetMeta();
			showView();
            break;
        }
    }

    protected override void showView (object param = null)
    {
		view = popupManagerDelegate.CreateAndAddPopup("UI/Legend/TransportPoint/TransportPoint") as TransportPointView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.detailButton1.gameObject).onClick = onDetailButtonClick;
		UIEventListener.Get(view.detailButton2.gameObject).onClick = onDetailButtonClick;
		UIEventListener.Get(view.detailButton3.gameObject).onClick = onDetailButtonClick;
		
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
		UIEventListener.Get(view.detailButton1.gameObject).onClick = null;
		UIEventListener.Get(view.detailButton2.gameObject).onClick = null;
		UIEventListener.Get(view.detailButton3.gameObject).onClick = null;
		UIEventListener.Get(view.closeButton.gameObject).onClick = null;
	}

    private void onCloseButtonClick(GameObject go)
    {
		hideView();
    }

    private void onDetailButtonClick(GameObject go)
    {
        SendNotification(NotificationConst.TRANSPORTPOINTDETAIL_SHOW);
    }
}

