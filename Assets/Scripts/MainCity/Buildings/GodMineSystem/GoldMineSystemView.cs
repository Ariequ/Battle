using UnityEngine;
using System.Collections;

public class GoldMineSystemView : BasePopupView 
{
    public UISprite closeButton;
    public UISprite detailButton;

    public UILabel label1;
    public UILabel label2;
    public UILabel label3;
    public UILabel label4;


    public void Start ()
    {
    }

    public void InitializeUI(int tax)
    {
//        taxLabel.text = tax.ToString();
    }

    public void UpdateUI(GoldMineSystemMeta meta)
    {
        label1.text =  meta.level + "级";
        label2.text = "+" + meta.smallEffect + "%";
        label3.text = "+" + meta.midEffect + "%";
        label4.text = "+" + meta.largeEffect + "%";
    }
}
