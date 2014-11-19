using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class ParliamentMediator : UIMediator
{
    new public static String NAME = "ParliamentMediator";

    private const CityBuildingType BUILDING_TYPE = CityBuildingType.Parliament;
    private const string PANEL_PATH = "UI/MainCity/ParliamentUI";

    private ParliamentView view;

    private ParliamentProxy proxy;

	private int tax;

    public ParliamentMediator() : base (NAME)
    {
        proxy = new ParliamentProxy();
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
        notifications.Add(NotificationConst.PARLIAMENT_SHOW);
        notifications.Add(NotificationConst.GET_TAX_RET);

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
            case NotificationConst.PARLIAMENT_SHOW:
            {
                tax = (int)(notification.Body);
                showView(tax);
                break;
            }
            case NotificationConst.GET_TAX_RET:
                tax = (int)(notification.Body);
                getTaxCallback(tax);
                break;
        }
    }

    protected override void showView (object param = null)
    {
		view = popupManagerDelegate.CreateAndAddPopup(PANEL_PATH) as ParliamentView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		view.InitializeUI(tax);
		UIEventListener.Get(view.closeButton.gameObject).onClick = onCloseButtonClick;
		UIEventListener.Get(view.mainButton.gameObject).onClick = onMainButtonClick;
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
		UIEventListener.Get(view.mainButton.gameObject).onClick = null;
	}

    private void onCloseButtonClick(GameObject go)
    {
        hideView();
    }

    private void onMainButtonClick(GameObject go)
    {
        if (!proxy.HasCollect)
        {
            proxy.GetTax();
        }
        else
        {
            AlertView alertView = AlertView.CreateAndShow();
            alertView.titleLabel.text = "Warning";
            alertView.contentLabel.text = "No more tax.";
        }
    }

    private void getTaxCallback(int tax)
    {
        LordProxy lordProxy = ApplicationFacade.Instance.RetrieveProxy(LordProxy.NAME) as LordProxy;
        lordProxy.ChangeCurrencyValue(CurrencyType.Gold, tax);

        view.UpdateUI(tax > 0);

        AlertView alertView = AlertView.CreateAndShow();
        alertView.titleLabel.text = "Success";
        alertView.contentLabel.text = "Tax collected.";
    }
}

