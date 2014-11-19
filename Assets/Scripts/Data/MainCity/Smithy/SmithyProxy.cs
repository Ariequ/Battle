using System;

using PureMVC.Patterns;

public class SmithyProxy : Proxy
{
    new public const string NAME = "SmithyProxy";

    public SmithyProxy() : base(NAME)
    {
    }

    public void GetData()
    {

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

