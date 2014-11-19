using System;

using PureMVC.Patterns;

public class StarPalaceProxy : Proxy
{
    new public const string NAME = "StarPalaceProxy";

    public StarPalaceProxy() : base(NAME)
    {
    }

    public void GetData()
    {
        int tax = 0;

        if (MockServer.isMocking)
        {
            tax = MockServer.GetParliamentData();
        }

        SendNotification(NotificationConst.STARPALACE_UPDATE, tax);
    }

    public GodnessMeta GetMeta()
    {
        GodnessMeta meta = new GodnessMeta();
        meta.level = RandomUtil.Range(1,10);
        meta.attack = RandomUtil.Range(10,200);
        meta.hp = RandomUtil.Range(30, 50);
        meta.defence = RandomUtil.Range(333,494);

        return meta;
    }
}

