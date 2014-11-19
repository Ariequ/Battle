using System;
using System.Collections.Generic;

using Battle;

public class UnitController
{
    private UnitVO unitVO;

    private SoldierMeta meta;

    private bool isCacheDirty;

    private int attackCache;
    private int defenseCache;
    private int hpCache;
    private int currentHP;
    private int luckCache;

    private Dictionary<PropertyType, int> buffPointDic = new Dictionary<PropertyType, int>();
    private Dictionary<PropertyType, float> buffPercentDic = new Dictionary<PropertyType, float>();

    private IBattleAgent agentDelegate;
    private ISkillContainerDelegate skillContainerDelegate;

	public UnitController(UnitVO unitVO, ISkillContainerDelegate skillContainerDelegate)
    {
        this.isCacheDirty = true;

        this.unitVO = unitVO;

        this.meta = unitVO.soldierVO.Meta;

        this.skillContainerDelegate = skillContainerDelegate;

        ResetBuffDic();
    }

    public IBattleAgent BattleAgentDelegate
    {
        set
        {
            if (agentDelegate == null)
            {
                agentDelegate = value;
            }
        }
    }

    public Faction Faction
    {
        get
        {
            return unitVO.Faction;
        }
    }

    public SoldierMeta Meta
    {
        get
        {
            return meta;
        }
    }

    public void Update()
    {
        float deltaTime = BattleTime.deltaTime;
        foreach (BuffVO buffVO in unitVO.buffVOList)
        {
            buffVO.remainTime -= deltaTime;
            if (buffVO.remainTime <= 0)
            {
                isCacheDirty = true;
            }
            else if (buffVO.meta.frequency > 0)
            {
                if (buffVO.remainFrequency > 0)
                {
                    buffVO.remainFrequency -= deltaTime;
                }
                else
                {
                    SkillLinkMeta[] skillLinkMetaArray = buffVO.meta.skillLinkMetaArray;
                    if (skillLinkMetaArray != null && skillLinkMetaArray.Length > 0)
                    {
                        skillContainerDelegate.ExecuteSkillsBehind(this, agentDelegate.Position, skillLinkMetaArray);
                        isCacheDirty = true;
                    }
                }
            }
        }
    }

    public void AddBuff(BuffMeta buffMeta)
    {
        isCacheDirty = true;

        foreach (BuffVO buffVO in unitVO.buffVOList)
        {
            if (buffVO.meta == buffMeta)
            {
                buffVO.Add();
                return;
            }
        }

        if (unitVO.buffVOList.Count < BattleValueConst.MAX_BUFF_COUNT)
        {
            BuffVO buffVO = new BuffVO(buffMeta);
            unitVO.buffVOList.Add(buffVO);
        }
    }
    
    public void RemoveBuff(BuffVO buffVO)
    {
        isCacheDirty = true;
        unitVO.buffVOList.Remove(buffVO);
    }
    
    private void CalculatePropertyCache()
    {
        attackCache = unitVO.Attack;
        defenseCache = unitVO.Defense;
        hpCache = unitVO.MaxHP;
        currentHP = unitVO.CurrentHP;
        luckCache = unitVO.Luck;
        
        ResetBuffDic();

        List<int> deleteIndexes = new List<int>();
        int count = unitVO.buffVOList.Count;

        for (int i = 0; i < count; ++i)
        {
            BuffVO buffVO = unitVO.buffVOList[i];
            
            if (buffVO.remainTime > 0)
            {
                BuffMeta buffMeta = buffVO.meta;
                if (buffMeta.type == BuffType.Property)
                {
                    buffPercentDic[buffMeta.propertyType] += buffVO.percent;
                    buffPointDic[buffMeta.propertyType] += buffVO.point;
                }
            }
            else
            {
                deleteIndexes.Add(i);
            }
        }

        attackCache = (int)(attackCache * buffPercentDic[PropertyType.Attack]) + buffPointDic[PropertyType.Attack];
        defenseCache = (int)(defenseCache * buffPercentDic[PropertyType.Defense]) + buffPointDic[PropertyType.Defense];
        hpCache = (int)(hpCache * buffPercentDic[PropertyType.HP]) + buffPointDic[PropertyType.HP];
        currentHP = (int)(currentHP * buffPercentDic[PropertyType.HP]) + buffPointDic[PropertyType.HP];
        luckCache = (int)(luckCache * buffPercentDic[PropertyType.Luck]) + buffPointDic[PropertyType.Luck];
        
        for (int i = 0; i < deleteIndexes.Count; ++i)
        {
            unitVO.buffVOList.RemoveAt(deleteIndexes[i]);
        }
        
        isCacheDirty = false;
    }

    public AttackValue CreateAttackValue()
    {
        AttackValue attackValue = new AttackValue();
        attackValue.faction = unitVO.Faction;
        attackValue.level = unitVO.soldierVO.level;
        attackValue.maxUnitCount = this.meta.maxUnitCount;
        
        attackValue.attack = unitVO.Attack;
        
        attackValue.luck = unitVO.Luck;
        attackValue.criticalDamage = meta.criticalDamage;
        
        attackValue.damagePoint = meta.damagePoint;
        attackValue.damagePercent = meta.damagePercent;
        
        return attackValue;
    }

    private void ResetBuffDic()
    {
        for (int i = 0; i < (int)PropertyType.None; ++i)
        {
            buffPointDic[(PropertyType)i] = 0;
            buffPercentDic[(PropertyType)i] = 1f;
        }
    }

    #region Properties

    public int Attack
    {
        get
        {
            if (isCacheDirty)
            {
                CalculatePropertyCache();
            }
            return attackCache;
        }
    }

    public int Defense
    {
        get
        {
            if (isCacheDirty)
            {
                CalculatePropertyCache();
            }
            return defenseCache;
        }
    }

    public int HP
    {
        get
        {
            if (isCacheDirty)
            {
                CalculatePropertyCache();
            }
            return currentHP;
        }
        set
        {
            currentHP = Math.Max(0, Math.Min(hpCache, value));
            unitVO.CurrentHP = currentHP;
        }
    }

    public int Luck
    {
        get
        {
            if (isCacheDirty)
            {
                CalculatePropertyCache();
            }
            return luckCache;
        }
    }

    #endregion

}

