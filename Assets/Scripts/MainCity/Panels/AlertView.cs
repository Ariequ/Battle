using UnityEngine;
using System.Collections;

public class AlertView : BasePopupView
{
    public const string PANEL_PATH = "UI/MainCity/Panels/MainCityAlert";

    public UILabel titleLabel;

    public UILabel contentLabel;

    public UISprite okButton;
    public UISprite closeButton;

    void Start()
    {
        UIEventListener.Get(okButton.gameObject).onClick += onOKButtonClick;
        UIEventListener.Get(closeButton.gameObject).onClick += onOKButtonClick;
    }

	public static AlertView CreateAndShow(string title, string content)
	{
		AlertView alertView = PopupManager.Instance.CreateAndAddPopup(PANEL_PATH, PopupMode.DEFAULT, PopupQueueMode.NoQueue) as AlertView;
		alertView.titleLabel.text = title;
		alertView.contentLabel.text = content;

		return alertView;
	}
	
    public static AlertView CreateAndShow()
    {
		AlertView alertView = PopupManager.Instance.CreateAndAddPopup(PANEL_PATH, PopupMode.DEFAULT, PopupQueueMode.NoQueue) as AlertView;

        return alertView;
    }

    protected virtual void onOKButtonClick(GameObject go)
    {
        Hide();
    }

    public void Hide()
    {
		UIEventListener.Get(okButton.gameObject).onClick -= onOKButtonClick;
		UIEventListener.Get(closeButton.gameObject).onClick -= onOKButtonClick;
		PopupManager.Instance.RemovePopup(this, true);
    }
}

