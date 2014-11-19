using System;

using PureMVC.Patterns;

public class TransportPointProxy : Proxy
{
    new public const string NAME = "TransportPointProxy";
    
    public TransportPointProxy() : base(NAME)
    {
    }
    
    public TransportPointMeta GetMeta()
    {
        TransportPointMeta meta = new TransportPointMeta();
        
        return meta;
    }
}