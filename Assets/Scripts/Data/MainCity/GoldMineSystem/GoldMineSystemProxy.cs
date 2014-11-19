using System;

using PureMVC.Patterns;

public class GoldMineSystemProxy : Proxy
{
    new public const string NAME = "GoldMineSystemProxy";

    public GoldMineSystemProxy() : base(NAME)
    {
    }

    public GoldMineSystemMeta GetMeta()
    {
        GoldMineSystemMeta meta = new GoldMineSystemMeta();
        meta.level = RandomUtil.Range(1,10);
        meta.smallEffect = RandomUtil.Range(0,10);
        meta.midEffect = RandomUtil.Range(10, 20);
        meta.largeEffect = RandomUtil.Range(20,30);

        return meta;
    }
}

