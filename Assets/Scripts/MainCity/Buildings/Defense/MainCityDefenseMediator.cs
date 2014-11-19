using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class MainCityDefenseMediator : UIMediator
{
    new public static String NAME = "MainCityDefenseMediator";

    private const CityBuildingType BUILDING_TYPE = CityBuildingType.Defense;
    private const string PANEL_PATH = "UI/MainCity/MainCityDefenseUI";

    private MainCityDefenseView view;

    private MainCityDefenseProxy proxy;

    public MainCityDefenseMediator() : base (NAME)
    {
        proxy = new MainCityDefenseProxy();
        ApplicationFacade.Instance.RegisterProxy(proxy);
    }

    public override void OnRemove()
    {
        ApplicationFacade.Instance.RemoveProxy(ParliamentProxy.NAME);
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();

        notifications.Add(NotificationConst.MAIN_CITY_INIT_PANEL);
        notifications.Add(NotificationConst.DEFENSE_SHOW);

        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name)
        {
            case NotificationConst.MAIN_CITY_INIT_PANEL:
            {
                CityBuildingType buildingType = (CityBuildingType)(notification.Body);
                if (buildingType == BUILDING_TYPE)
                    proxy.GetData();
                break;
            }
            case NotificationConst.DEFENSE_SHOW:
            {
                showView();
                break;
            }
        }
    }

    protected override void showView (object param = null)
    {
    	view = popupManagerDelegate.CreateAndAddPopup(PANEL_PATH) as MainCityDefenseView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.closeButton.gameObject).onClick = onCloseButtonClick;
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
}

