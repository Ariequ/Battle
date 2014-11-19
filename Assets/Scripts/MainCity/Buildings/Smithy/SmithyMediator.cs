using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class SmithyMediator : UIMediator
{
    new public static String NAME = "SmithyMediator";

	public const string PANEL_PATH = "UI/MainCity/Smithy/SmithyView";

    private SmithyView view;
    private SmithyProxy proxy;
    public const CityBuildingType BUILDING_TYPE = CityBuildingType.Smithy;

    public SmithyMediator() : base (NAME)
    {
        proxy = new SmithyProxy();
        ApplicationFacade.Instance.RegisterProxy(proxy);
    }

    public override void OnRemove()
    {
        ApplicationFacade.Instance.RemoveProxy(SmithyProxy.NAME);
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
                    showView();
                }
                break;
            }
        }
    }

    protected override void showView (object param = null)
    {
		view = popupManagerDelegate.CreateAndAddPopup(PANEL_PATH) as SmithyView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.closeButton.gameObject).onClick = onCloseButtonClick;
		view.InitializeUI();
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

