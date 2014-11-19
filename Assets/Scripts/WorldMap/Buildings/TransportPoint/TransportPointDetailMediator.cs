using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class TransportPointDetailMediator : UIMediator
{
    new public static String NAME = "TransportPointDetailMediator";
    private TransportPointDetailView view;
    private TransportPointDetailProxy proxy;

	private TransportPointMeta meta;

    public TransportPointDetailMediator() : base (NAME)
    {
        proxy = new TransportPointDetailProxy();
        ApplicationFacade.Instance.RegisterProxy(proxy);
    }

    public override void OnRemove()
    {
        ApplicationFacade.Instance.RemoveProxy(TransportPointDetailProxy.NAME);
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();

        notifications.Add(NotificationConst.TRANSPORTPOINTDETAIL_SHOW);

        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
        case NotificationConst.TRANSPORTPOINTDETAIL_SHOW:
			meta = proxy.GetMeta();
			showView();
            break;
        }
    }

    protected override void showView (object param = null)
    {
		view = (TransportPointDetailView)popupManagerDelegate.CreateAndAddPopup("UI/Legend/TransportPoint/TransportPointDetail", PopupMode.DEFAULT, PopupQueueMode.QueueFrontShow);
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.returnBtn.gameObject).onClick = onReturnButtonClick;
		UIEventListener.Get(view.transportButton1.gameObject).onClick = onTransportButtonClick;
		UIEventListener.Get(view.transportButton2.gameObject).onClick = onTransportButtonClick;
		UIEventListener.Get(view.transportButton3.gameObject).onClick = onTransportButtonClick;
		
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
		UIEventListener.Get(view.returnBtn.gameObject).onClick = null;
		UIEventListener.Get(view.transportButton1.gameObject).onClick = null;
		UIEventListener.Get(view.transportButton2.gameObject).onClick = null;
		UIEventListener.Get(view.transportButton3.gameObject).onClick = null;
	}

    private void onReturnButtonClick(GameObject go)
    {
		hideView();
    }

    private void onTransportButtonClick(GameObject go)
    {
        GameGlobal.LoadSceneByName(SceneNameConst.LEGEND_SCENE_ + "02_02_02");
    }
}
