using UnityEngine;
using System.Collections;

public class ParliamentView : BasePopupView 
{
    public UISprite closeButton;
    public UISprite mainButton;

    public UILabel taxLabel;

    public void InitializeUI(int tax)
    {
        taxLabel.text = tax.ToString();
    }

    public void UpdateUI(bool getTaxResult)
    {
        if (getTaxResult)
            Debug.Log("Get tax successfully!");
        else
            Debug.Log("Failed to get tax!");
    }
}
