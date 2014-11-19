using System;
using PureMVC.Patterns;

public class ParliamentProxy : Proxy
{
    new public const string NAME = "ParliamentProxy";

    public bool HasCollect { get; private set; }

    public ParliamentProxy() : base(NAME)
    {
        HasCollect = false;
    }

    public void GetData()
    {
        int tax = 0;

        if (MockServer.isMocking)
        {
            tax = MockServer.GetParliamentData();
        }

        SendNotification(NotificationConst.PARLIAMENT_SHOW, tax);
    }

    public void GetTax()
    {
        int tax = 0;
        
        if (MockServer.isMocking)
        {
            tax = MockServer.GetTax();

            if (tax > 0)
            {
                HasCollect = true;
                SendNotification(NotificationConst.GET_TAX_RET, tax);
            }
        }
        
    }
}

