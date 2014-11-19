using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class UpgradeMediator : UIMediator
{
    new public static string NAME = "UpgradeMediator";

    private const string PANEL_PATH = "UI/MainCity/UpgradeBuildingUI";

    private UpgradeView view;
    private UpgradeProxy proxy;

    private int buildingMetaID;
	private Dictionary<string, object> data;

    public UpgradeMediator() : base(NAME)
    {
        proxy = new UpgradeProxy();
        ApplicationFacade.Instance.RegisterProxy(proxy);
    }

    public override void OnRemove()
    {
        ApplicationFacade.Instance.RemoveProxy(UpgradeProxy.NAME);
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();
        
        notifications.Add(NotificationConst.MAIN_CITY_UPGRADE_PANEL);
        notifications.Add(NotificationConst.UPGRADE_SHOW);
        notifications.Add(NotificationConst.UPGRADE_BUILDING);
        
        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name)
        {
            case NotificationConst.MAIN_CITY_UPGRADE_PANEL:
                buildingMetaID = (int)(notification.Body);
                proxy.GetData(buildingMetaID);
                break;
            case NotificationConst.UPGRADE_SHOW:
                data = notification.Body as Dictionary<string, object>;
                showView(data);
                break;
            case NotificationConst.UPGRADE_BUILDING:
                data = notification.Body as Dictionary<string, object>;
                upgradeBuildingCallback();
                break;
        }
    }

    protected override void showView (object param = null)
    {
    	view = popupManagerDelegate.CreateAndAddPopup(PANEL_PATH) as UpgradeView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		bool isUpgrading = (bool)(data[UpgradeProxy.IS_UPGRADING]);
		long remainTime = (long)(data[UpgradeProxy.REMAIN_TIME]);
		view.UpdateUI(buildingMetaID, isUpgrading, remainTime);
		
		UIEventListener.Get(view.closeButton.gameObject).onClick = onCloseButtonClick;
		UIEventListener.Get(view.upgradeButton.gameObject).onClick = onUpgradeButtonClick;
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
		UIEventListener.Get(view.upgradeButton.gameObject).onClick = null;
	}

    private void onCloseButtonClick(GameObject go)
    {
        hideView();
    }

    private void onUpgradeButtonClick(GameObject go)
    {
        hideView();
//        proxy.UpgradeBuilding(buildingMetaID);
    }

    private void upgradeBuildingCallback()
    {
        int nextMetaID = (int)(data[UpgradeProxy.META_ID]);
        bool isUpgrading = (bool)(data[UpgradeProxy.IS_UPGRADING]);
        long remainTime = (long)(data[UpgradeProxy.REMAIN_TIME]);
        buildingMetaID = nextMetaID;

        view.UpdateUI(buildingMetaID, isUpgrading, remainTime);
    }
}

