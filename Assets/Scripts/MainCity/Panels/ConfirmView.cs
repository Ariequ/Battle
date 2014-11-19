using UnityEngine;
using System.Collections;

public class ConfirmView : BasePopupView
{
    public const string PANEL_PATH = "UI/MainCity/Panels/MainCityConfirm";
    
    public UILabel titleLabel;
    
    public UILabel contentLabel;
    
    public UISprite confirmButton;

    public UISprite cancelButton;

    void Start()
    {
        UIEventListener.Get(confirmButton.gameObject).onClick += onOKButtonClick;
		UIEventListener.Get(cancelButton.gameObject).onClick += onCancelButtonClick;
    }
	
    public static ConfirmView CreateAndShow()
    {
		ConfirmView confirmView = PopupManager.Instance.CreateAndAddPopup(PANEL_PATH, PopupMode.DEFAULT, PopupQueueMode.NoQueue) as ConfirmView;
        return confirmView;
    }

	protected virtual void onOKButtonClick(GameObject go)
    {
        Hide();
    }

	protected virtual void onCancelButtonClick(GameObject go)
	{
		Hide();
	}
    
    public void Hide()
    {
		UIEventListener.Get(confirmButton.gameObject).onClick -= onOKButtonClick;
		UIEventListener.Get(cancelButton.gameObject).onClick -= onCancelButtonClick;
		PopupManager.Instance.RemovePopup(this, true);
    }
}

