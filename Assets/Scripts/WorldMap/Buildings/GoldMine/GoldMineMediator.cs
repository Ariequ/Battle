using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class GoldMineMediator : UIMediator
{
    new public static String NAME = "GoldMineMediator";

    public const int ID = 2;

    private GoldMineView view;
    private GoldMineProxy proxy;
	private GoldMineMeta meta;

    public GoldMineMediator() : base (NAME)
    {
        proxy = new GoldMineProxy();
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
		view = popupManagerDelegate.CreateAndAddPopup("UI/Legend/GoldMine/GoldMine") as GoldMineView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.ocuppyBtn.gameObject).onClick = onOcuppyButtonClick;
		UIEventListener.Get(view.closeBtn.gameObject).onClick = onOcuppyButtonClick;
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
		UIEventListener.Get(view.ocuppyBtn.gameObject).onClick = null;
		UIEventListener.Get(view.closeBtn.gameObject).onClick = null;
	}

    private void onOcuppyButtonClick(GameObject go)
    {
		hideView();
    }
}

