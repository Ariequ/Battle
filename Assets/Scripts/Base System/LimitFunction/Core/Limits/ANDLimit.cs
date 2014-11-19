public class ANDLimit : ILimit
{
    private int _limitID;
    private ILimit _fistLimit;
    private ILimit _secondLimit;

    public ANDLimit (int limitID, ILimit firstLimit, ILimit secondLimit)
    {
        _limitID = limitID;
        _fistLimit = firstLimit;
        _secondLimit = secondLimit;
    }

    public int getLimitID ()
    {
        return _limitID;
    }

    public bool IsLimitConfirm(ILimitFuncitonContext context)
    {
        return _fistLimit.IsLimitConfirm(context) & _secondLimit.IsLimitConfirm(context);
    }
}
