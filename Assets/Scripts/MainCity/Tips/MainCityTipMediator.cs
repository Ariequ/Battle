using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class MainCityTipMediator : UIMediator
{
    new public static String NAME = "TipMediator";

    private const string BUILDING_TIP_PATH = "UI/MainCity/Panels/BuildingTip";
    private const string UPGRADE_TIP_PATH = "UI/MainCity/Panels/UpgradeTip";

    private BuildingTipView buildingTipView;
    private UpgradeTipView upgradeTipView;

    private MainCityTipDTO dto;

    public MainCityTipMediator() : base (NAME)
    {
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();

        notifications.Add(NotificationConst.MAIN_CITY_TIP_SHOW);
        
        return notifications;
    }
    
    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name)
        {
            case NotificationConst.MAIN_CITY_TIP_SHOW:
            {
				this.dto = (MainCityTipDTO)(notification.Body);
				showView(dto);
                break;
            }
            
        }
    }

    protected override void showView(object param = null)
    {
        switch(dto.buildingState)
        {
            case MainCityBuildingState.Available:
            {
				Dictionary<uint, object> paramDic = new Dictionary<uint, object>();
				paramDic[PopupMode.ADD_MASK] = true;
				buildingTipView = popupManagerDelegate.CreateAndAddPopup(BUILDING_TIP_PATH, PopupMode.DEFAULT, PopupQueueMode.NoQueue, paramDic) as BuildingTipView;

                UIEventListener.Get(buildingTipView.visitButton.gameObject).onClick = onVisitButtonClick;
                UIEventListener.Get(buildingTipView.buildUpgradeButton.gameObject).onClick = onUpgradeButtonClick;

                break;
            }
            case MainCityBuildingState.IsUpgrading:
            {
                break;
            }
        }
    }

	protected override void updateViewShow ()
	{
	}

    protected override void hideView (object param = null)
    {
	    UIEventListener.Get(buildingTipView.visitButton.gameObject).onClick = null;
	    UIEventListener.Get(buildingTipView.buildUpgradeButton.gameObject).onClick = null;
	    popupManagerDelegate.RemovePopup(buildingTipView);
    }

	protected override void updateViewHide ()
	{
	}

    private void onVisitButtonClick(GameObject go)
    {
		hideView();

        if (!Facade.HasMediator(dto.buildingMediator.MediatorName))
        {
            Facade.RegisterMediator(dto.buildingMediator);
        }

        SendNotification(NotificationConst.MAIN_CITY_INIT_PANEL, dto.buildingType);
    }

    private void onUpgradeButtonClick(GameObject go)
    {
		hideView();

        if (!Facade.HasMediator(UpgradeMediator.NAME))
        {
            Facade.RegisterMediator(new UpgradeMediator());
        }
        SendNotification(NotificationConst.MAIN_CITY_UPGRADE_PANEL, dto.buildingMetaID);
    }
}

