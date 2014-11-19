using System;
using PureMVC.Patterns;

public class LordProxy : Proxy
{
    new public const string NAME = "LordProxy";

    private LordVO lordVO;

    public LordProxy() : base(NAME)
    {
        lordVO = MockServer.lordVO;
    }

    public void ChangeCurrencyValue(CurrencyType type, int value)
    {
        int newValue = 0;
        switch(type)
        {
            case CurrencyType.Gem:
                lordVO.gem += value;
                newValue = lordVO.gem;
                break;
            case CurrencyType.Gold:
                lordVO.gold += value;
                newValue = lordVO.gold;
                break;
            case CurrencyType.Wood:
                lordVO.wood += value;
                newValue = lordVO.wood;
                break;
            case CurrencyType.Ore:
                lordVO.ore += value;
                newValue = lordVO.ore;
                break;
        }

        CurrencyChangeDTO dto = new CurrencyChangeDTO();
        dto.type = type;
        dto.newValue = newValue;

        SendNotification(NotificationConst.CURRENCY_VALUE_CHANGE, dto);
    }

    public string LordName
    {
        get
        {
            return lordVO.lordName;
        }
    }

    public int Level
    {
        get
        {
            return lordVO.level;
        }
    }

    public long Exp
    {
        get
        {
            return lordVO.exp;
        }
    }

    public int Energy
    {
        get
        {
            return lordVO.energy;
        }
    }

    public int Gem
    {
        get
        {
            return lordVO.gem;
        }
    }

    public int Gold
    {
        get
        {
            return lordVO.gold;
        }
    }

    public int Wood
    {
        get
        {
            return lordVO.wood;
        }
    }

    public int Ore
    {
        get
        {
            return lordVO.ore;
        }
    }

    public int VIPLevel
    {
        get
        {
            return lordVO.vipLevel;
        }
    }
}

