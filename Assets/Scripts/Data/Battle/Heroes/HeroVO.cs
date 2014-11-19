using System;
using System.Collections.Generic;

public class HeroVO
{
    public int level = 1;

    public int starLevel = 1;

    public int buffCount;

	public int id;

	private HeroMeta meta;

	private Faction faction;

    private bool isCacheDirty = true;

    private int attackCache;



	public HeroVO (HeroMeta meta, Faction faction)
	{
		this.meta = meta;
		this.faction = faction;
	}
	public int ID 
	{
		get
		{
			return this.meta.id;
		}
	}

	public HeroMeta Meta
	{
		get
		{
			return this.meta;
		}
	}

	public Faction Faction
	{
		get
		{
			return this.faction;
		}
	}

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

    private int defenseCache;
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

    private int hpCache;
    public int HP
    {
        get
        {
            if (isCacheDirty)
            {
                CalculatePropertyCache();
            }
            return hpCache;
        }
    }



    private void CalculatePropertyCache()
    {
        attackCache = meta.initialAttack + (int)(meta.attackIncrement * level);
        defenseCache = meta.initialDefense + (int)(meta.defenseIncrement * level);
        hpCache = meta.initialHP + (int)(meta.hpIncrement * level);

//        foreach (EquipmentVO equipmentVO in equipmentList)
//        {
//            attackCache += equipmentVO.Attack;
//            defenseCache += equipmentVO.Defense;
//            hpCache += equipmentVO.HP;
//        }

        isCacheDirty = false;
    }

    public int SkillID
    {
        get
        {
            return meta.skillDic[starLevel];
        }
    }

//    private List<EquipmentVO> equipmentList = new List<EquipmentVO>();
//    
//    public void AddEquipment(EquipmentVO equipmentVO)
//    {
//        if (equipmentList.Count < BattleValueConst.MAX_EQUIPMENT_COUNT)
//        {
//            equipmentList.Add(equipmentVO);
//        }
//    }

//    public EquipmentVO[] GetEquipmentArray()
//    {
//        return equipmentList.ToArray();
//    }

	public int Fighting
	{
		get
		{
			if (isCacheDirty)
			{
				CalculatePropertyCache();
			}
			return (Attack+Defense+HP)*starLevel;
		}
	}
}

