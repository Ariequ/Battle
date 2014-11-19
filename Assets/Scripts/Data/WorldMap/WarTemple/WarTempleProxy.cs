using System;

using PureMVC.Patterns;

public class WarTempleProxy : Proxy
{
    new public const string NAME = "WarTempleProxy";
    
    public WarTempleProxy() : base(NAME)
    {
    }
    
    public WarTempleMeta GetMeta(int id)
    {
        WarTempleMeta meta = new WarTempleMeta();
        
        return meta;
    }
}