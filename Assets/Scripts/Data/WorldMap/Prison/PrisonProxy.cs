using System;

using PureMVC.Patterns;

public class PrisonProxy : Proxy
{
    new public const string NAME = "PrisonProxy";
    
    public PrisonProxy() : base(NAME)
    {
    }
    
    public PrisonMeta GetMeta()
    {
        PrisonMeta meta = new PrisonMeta();
        
        return meta;
    }
}