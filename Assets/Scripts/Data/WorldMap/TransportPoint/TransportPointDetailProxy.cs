using System;

using PureMVC.Patterns;

public class TransportPointDetailProxy : Proxy
{
    new public const string NAME = "TransportPointDetailProxy";
    
    public TransportPointDetailProxy() : base(NAME)
    {
    }
    
    public TransportPointMeta GetMeta()
    {
        TransportPointMeta meta = new TransportPointMeta();
        
        return meta;
    }
}