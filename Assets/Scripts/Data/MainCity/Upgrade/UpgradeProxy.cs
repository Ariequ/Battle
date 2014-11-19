using System;
using System.Collections.Generic;

using PureMVC.Patterns;

public class UpgradeProxy : Proxy
{
    new public const string NAME = "UpgradeProxy";

    public const string META_ID = "META_ID";
    public const string IS_UPGRADING = "IS_UPGRADING";
    public const string REMAIN_TIME = "REMAIN_TIME";
    
    public UpgradeProxy()
    {
    }

    public void GetData(int buildingMetaID)
    {
        Dictionary<string, object> data = null;

        if (MockServer.isMocking)
        {
            data = MockServer.GetBuildingUpgradeData(buildingMetaID);
        }
        SendNotification(NotificationConst.UPGRADE_SHOW, data);
    }

    public void UpgradeBuilding(int buildingMetaID)
    {
        Dictionary<string, object> data = null;
        
        if (MockServer.isMocking)
        {
            data = MockServer.UpgradeBuilding(buildingMetaID);
        }
        SendNotification(NotificationConst.UPGRADE_BUILDING, data);
    }
}

