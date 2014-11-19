using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class MainCityUIMediator : Mediator
{
    new public static String NAME = "MainCityUIMediator";

    private MainCityUIView view;

    private LordProxy lordProxy;

    public MainCityUIMediator() : base (NAME)
    {
        lordProxy = ApplicationFacade.Instance.RetrieveProxy(LordProxy.NAME) as LordProxy;

        view = GameObject.Find("MainCityMainUI").GetComponent<MainCityUIView>();
        UIEventListener.Get(view.leaveButton.gameObject).onClick = onLeaveButtonClick;
		UIEventListener.Get(view.heroUIButton.gameObject).onClick = onHeroUIButtonClick;

        updateView();
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();
        notifications.Add(NotificationConst.CURRENCY_VALUE_CHANGE);
        return notifications;
    }
    
    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name)
        {
            case NotificationConst.CURRENCY_VALUE_CHANGE:
            {
                CurrencyChangeDTO dto = (CurrencyChangeDTO)(notification.Body);
                handleCurrencyValueChange(dto);
                break;
            }
        }
    }

    private void updateView()
    {
        view.lordNameLabel.text = lordProxy.LordName;
        view.levelLabel.text = lordProxy.Level.ToString();
        view.expLabel.text = lordProxy.Exp.ToString() + "/" + MetaManager.Instance.GetLordLevelExp(lordProxy.Level).ToString();
        view.energyLabel.text = lordProxy.Energy.ToString() + "/100";
        view.gemLabel.text = lordProxy.Gem.ToString();
        view.goldLabel.text = lordProxy.Gold.ToString();
        view.woodLabel.text = lordProxy.Wood.ToString();
        view.oreLabel.text = lordProxy.Ore.ToString();
        view.vipLevelLabel.text = "VIP" + lordProxy.VIPLevel.ToString();
        
    }

    private void handleCurrencyValueChange(CurrencyChangeDTO dto)
    {
        switch(dto.type)
        {
            case CurrencyType.Gem:
                view.gemLabel.text = dto.newValue.ToString();
                break;
            case CurrencyType.Gold:
                view.goldLabel.text = dto.newValue.ToString();
                break;
            case CurrencyType.Wood:
                view.woodLabel.text = dto.newValue.ToString();
                break;
            case CurrencyType.Ore:
                view.oreLabel.text = dto.newValue.ToString();
                break;
        }
    }

    private void onLeaveButtonClick(GameObject go)
    {
        ConfirmView confirmView = ConfirmView.CreateAndShow();
        confirmView.titleLabel.text = "Confirmation";
        confirmView.contentLabel.text = "Are you sure to leave?";

        UIEventListener.Get(confirmView.confirmButton.gameObject).onClick += onLeaveConfirm;
    }

	private void onHeroUIButtonClick(GameObject go)
	{
		SendNotification(NotificationConst.HEROUI_MAIN_SHOW_PANEL);
	}

    private void onLeaveConfirm(GameObject go)
    {
        GameGlobal.LoadSceneByName(SceneNameConst.MAIN_INTERFACE);
    }
}

