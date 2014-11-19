using System.Collections.Generic;
using UnityEngine;
using System;

public class LimitFunctionNode
{
    private ILimit _limit; //can be null;
    private List<LimitFunctionNode> _childList; //can't be null
    private int _id;
   
    Action<int,ILimitFuncitonContext> completeCallBack;

    public LimitFunctionNode (int id, ILimit limit, List<LimitFunctionNode> childList)
    {
        _id = id;
        _limit = limit;
        _childList = childList;
    }

    public bool hasChildNode (int jumpIndex)
    {
        return _childList != null && jumpIndex < _childList.Count;
    }

    public int GetID()
    {
        return _id;
    }

    public ILimit GetLimit()
    {
        return _limit;
    }

    public List<LimitFunctionNode> GetChildList()
    {
        return _childList;
    }

    public void Execute (ILimitFuncitonContext context, Action<int,ILimitFuncitonContext> completeCallBack)
    {
        if (_limit.IsLimitConfirm (context)) {
            LimitFunctionNode child = _childList[0];
            _childList.RemoveAt(0);
            RunChild (child, context, this.OnChildComplete);
        }
        else
        {
            completeCallBack(0, context);
        }
    }

    private void RunChild (LimitFunctionNode node, ILimitFuncitonContext context, Action<int,ILimitFuncitonContext> completeCallBack)
    {
        if (node.GetLimit() == null || node.GetLimit().IsLimitConfirm(context) && node.GetChildList().Count == 0)
        {
            IFunction function = FunctionFactory.Instance.Create(1);
            function.Execute(context,completeCallBack);
        }
        else if (node.GetLimit() != null && !node.GetLimit().IsLimitConfirm(context))
        {
            completeCallBack(0, context);
        }
        else
        {
            node.Execute(context, node.OnChildComplete);
        }
    }

    private void OnChildComplete (int jumpIndex, ILimitFuncitonContext context)
    {
        Debug.Log("on Child Complete :" + this.GetID());
        if (hasChildNode(jumpIndex))
        {
            LimitFunctionNode child = _childList[jumpIndex];
            _childList.RemoveAt(jumpIndex);
            RunChild(child, context, child.OnChildComplete);
        }
    }
}
