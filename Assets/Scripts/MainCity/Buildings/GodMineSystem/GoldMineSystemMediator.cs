using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class GoldMineSystemMediator : UIMediator
{
    new public static String NAME = "GoldMineSystemMediator";

    public const string PANEL_PATH = "UI/MainCity/GoldMineSystem";

    public const CityBuildingType BUILDING_TYPE = CityBuildingType.Goldmine;

    private GoldMineSystemView view;
    private GoldMineSystemProxy proxy;

	private GoldMineSystemMeta meta;

    public GoldMineSystemMediator() : base (NAME)
    {
        proxy = new GoldMineSystemProxy();
        ApplicationFacade.Instance.RegisterProxy(proxy);
    }

    public override void OnRemove()
    {
        ApplicationFacade.Instance.RemoveProxy(GoldMineSystemProxy.NAME);
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();

        notifications.Add(NotificationConst.MAIN_CITY_INIT_PANEL);

        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case NotificationConst.MAIN_CITY_INIT_PANEL:
            {
                CityBuildingType type = (CityBuildingType)(notification.Body);
                if (type == BUILDING_TYPE)
                {
					meta = proxy.GetMeta();
                    showView(proxy.GetMeta());
                }
                break;
            }
        }
    }

    protected override void showView (object param = null)
    {
		view = popupManagerDelegate.CreateAndAddPopup(PANEL_PATH) as GoldMineSystemView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.closeButton.gameObject).onClick = onCloseButtonClick;
		UIEventListener.Get(view.detailButton.gameObject).onClick = onDetailButtonClick;
		
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
		UIEventListener.Get(view.closeButton.gameObject).onClick = null;
		UIEventListener.Get(view.detailButton.gameObject).onClick = null;
	}

    private void onCloseButtonClick(GameObject go)
    {
        hideView();
    }

    private void onDetailButtonClick(GameObject go)
    {

    }
}
