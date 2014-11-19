using System;

using PureMVC.Patterns;

public class GoldMineProxy : Proxy
{
    new public const string NAME = "GoldMineProxy";
    
    public GoldMineProxy() : base(NAME)
    {
    }
    
    public GoldMineMeta GetMeta()
    {
        GoldMineMeta meta = new GoldMineMeta();
        
        return meta;
    }
}