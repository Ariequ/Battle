using System;
using System.Collections.Generic;

using PureMVC.Patterns;

public class MainCityDefenseProxy : Proxy
{
    new public const string NAME = "MainCityDefenseProxy";
    
    public MainCityDefenseProxy() : base(NAME)
    {
    }

    public void GetData()
    {
        SendNotification(NotificationConst.DEFENSE_SHOW);
    }
}

