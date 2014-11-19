using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class MecaneryBaseMediator : UIMediator
{
    new public static String NAME = "MecaneryBaseMediator";
    
    public const string PANEL_PATH = "UI/Legend/MercaneryBase/MercaneryBaseUI";
    
    public const int ID = 3;
    
    private MercaneryBaseView view;
    
    public MecaneryBaseMediator() : base (NAME)
    {
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
				showView();
			}
            break;
        }
    }
    
    protected override void showView (object param = null)
    {
		view = popupManagerDelegate.CreateAndAddPopup(PANEL_PATH) as MercaneryBaseView;
		view.UpdateShowDelegate = updateViewShow;
		view.UpdateHideDelegate = updateViewHide;
    }

	protected override void updateViewShow ()
	{
		UIEventListener.Get(view.closeButton.gameObject).onClick = onCloseButtonClick;
		view.UpdateUI();
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
