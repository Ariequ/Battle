using System;
using System.Collections.Generic;

public class UnitVO
{
    public SoldierVO soldierVO;

    public List<BuffVO> buffVOList = new List<BuffVO>();

    private Faction faction;

    public UnitVO(SoldierVO soldierVO)
    {
        this.soldierVO = soldierVO;
        this.faction = soldierVO.Faction;

        BattleValueProxy valueProxy = ApplicationFacade.Instance.RetrieveValueProxy(faction);
        
        attack = soldierVO.Attack + valueProxy.HeroAttack;
        defense = soldierVO.Defense + valueProxy.HeroDefense;
        maxHP = (soldierVO.HP + valueProxy.HeroHP) / soldierVO.Meta.maxUnitCount;
        currentHP = maxHP;
        luck = soldierVO.Luck + valueProxy.GetLordLuck();
    }

    #region Basic Properties

    public Faction Faction
    {
        get
        {
            return faction;
        }
    }

    private int attack;
    public int Attack
    {
        get
        {
            return attack;
        }
    }

    private int defense;
    public int Defense
    {
        get
        {
            return defense;
        }
    }

    private int maxHP;
    public int MaxHP
    {
        get
        {
            return maxHP;
        }
        set
        {
            currentHP = value;
        }
    }

    private int currentHP;
    public int CurrentHP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = value;
        }
    }

    private int luck;
    public int Luck
    {
        get
        {
            return luck;
        }
    }

    #endregion
}

