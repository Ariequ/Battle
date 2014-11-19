using System;
public interface IFunction
{
    int getLimitID();
    void Execute(ILimitFuncitonContext context, Action<int,ILimitFuncitonContext> callBack);
}
