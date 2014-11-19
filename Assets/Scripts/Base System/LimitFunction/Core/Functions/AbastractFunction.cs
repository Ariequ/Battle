using UnityEngine;
using System.Collections;
using System;

public class AbastractFunction : IFunction 
{
    private int _functionID;
    public AbastractFunction(int functionID)
    {
        _functionID = functionID;
    }

    public int getLimitID()
    {
        return _functionID;
    }

    public void Execute(ILimitFuncitonContext context, Action<int,ILimitFuncitonContext> callBack)
    {
        Debug.Log("Execute AbastractFunction");

        callBack(5, context);
    }
}
