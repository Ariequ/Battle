using UnityEngine;
using System.Collections;

public class AbstractLimit : ILimit
{
    private int _limitID;

    public AbstractLimit(int limitID)
    {
        _limitID = limitID;
    }

    public int getLimitID ()
    {
        return _limitID;
    }
    
    public bool IsLimitConfirm(ILimitFuncitonContext context)
    {
        return true;
    }
}
