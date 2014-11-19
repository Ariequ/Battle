using System;

using PureMVC.Patterns;

public class WinnowerProxy : Proxy
{
    new public const string NAME = "WinnowerProxy";
    
    public WinnowerProxy() : base(NAME)
    {
    }
    
    public WinnowerMeta GetMeta()
    {
        WinnowerMeta meta = new WinnowerMeta();
        
        return meta;
    }
}