using UnityEngine;
using System.Collections;

public class StarPalaceView : BasePopupView 
{
    public UISprite closeButton;
    public UISprite worshipButton;

    public UILabel label1;
    public UILabel label2;
    public UILabel label3;

    public UILabel label4;
    public UILabel label5;
    public UILabel label6;

    public UILabel label7;
    public UILabel label8;
    public UILabel label9;

    public void InitializeUI(int tax)
    {
//        taxLabel.text = tax.ToString();
    }

    public void UpdateUI(GodnessMeta meta, GodnessMeta meta2)
    {
        label1.text = "" + meta.hp;
        label2.text = "" + meta.attack;
        label3.text = "" + meta.defence;

        label4.text = "" + meta.hp;
        label5.text = "" + meta.attack;
        label6.text = "" + meta.defence;

        label7.text =  "+" + meta2.hp;
        label8.text = "+" + meta2.attack;
        label9.text = "+" + meta2.defence;
    }
}
