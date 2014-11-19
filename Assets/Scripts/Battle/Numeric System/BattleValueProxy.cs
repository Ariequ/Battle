using UnityEngine;

using System;
using System.Collections.Generic;

using PureMVC.Patterns;

public class BattleValueProxy : Proxy
{
    new public const string NAME = "BattleValueProxy";

    private const int MAX_ANGER = 100;
    private const int MAX_DELTA_ANGER = 3;

    private int anger;
    private int deltaAnger;

    private Dictionary<int, HeroVO> heroVODic;
    private Dictionary<int, SkillMeta> skillDic;
    private Dictionary<int, EquipmentVO> equipmentDic;

    private bool isCacheDirty;

    //For querying.
	private List<HeroVO> __heroVOList = null;
    private List<SkillMeta> __skillVOList = null;

    public BattleValueProxy (Faction faction) : base(NAME + faction.ToString())
    {
		heroVODic = new Dictionary<int, HeroVO>();
        skillDic = new Dictionary<int, SkillMeta>();
		equipmentDic = new Dictionary<int, EquipmentVO>();
		
		isCacheDirty = true;
    }

	public void InitializeBattleHeroes (HeroVO[] heroes)
	{
		heroVODic.Clear();
		skillDic.Clear ();
		equipmentDic.Clear ();

		__heroVOList = null;
		__skillVOList = null;

		isCacheDirty = true;

		foreach (HeroVO heroVO in heroes)
		{
			if (heroVO != null)
			{
				SkillMeta skillMeta = MetaManager.Instance.GetSkillMeta(heroVO.SkillID);
				
				heroVODic[heroVO.Meta.id] = heroVO;
                skillDic[skillMeta.id] = skillMeta;
			}
		}
	}

    public List<HeroVO> GetHeroVOList()
    {
		if (__heroVOList == null)
        {
			__heroVOList = new List<HeroVO>(heroVODic.Values);
        }
		return __heroVOList;
    }
    
    public List<SkillMeta> GetSkillMetaList()
    {
		if (__skillVOList == null)
        {
            __skillVOList = new List<SkillMeta>(skillDic.Values);
        }
        return __skillVOList;
    }

    private int heroAttackCache;
    public int HeroAttack
    {
        get
        {
            if (isCacheDirty)
            {
                CalculatePropertyCache();
            }
            return heroAttackCache;
        }
    }

    private int heroDefenseCache;
    public int HeroDefense
    {
        get
        {
            if (isCacheDirty)
            {
                CalculatePropertyCache();
            }
            return heroDefenseCache;
        }
    }

    private int heroHPCache;
    public int HeroHP
    {
        get
        {
            if (isCacheDirty)
            {
                CalculatePropertyCache();
            }
            return heroHPCache;
        }
    }

    public int GetLordLuck ()
    {
        //For test
        return 50;
    }

    private void CalculatePropertyCache()
    {
        heroAttackCache = 0;
        heroDefenseCache = 0;
        heroHPCache = 0;

        foreach (HeroVO heroVO in heroVODic.Values)
        {
            heroAttackCache += heroVO.Attack;
            heroDefenseCache += heroVO.Defense;
            heroHPCache += heroVO.HP;

            EquipmentVO[] equipmentVOArray = heroVO.GetEquipmentArray();
            foreach(EquipmentVO equipmentVO in equipmentVOArray)
            {
                heroAttackCache += equipmentVO.Attack;
                heroDefenseCache += equipmentVO.Defense;
                heroHPCache += equipmentVO.HP;
            }
        }

        isCacheDirty = false;
    }

    public HeroVO GetHeroVO (int metaID)
    {
        HeroVO heroVO = null;
        heroVODic.TryGetValue(metaID, out heroVO);

        return heroVO;
    }

//    public SkillVO GetHeroSkill (int skillID)
//    {
//        SkillVO skillVO = null;
//        skillDic.TryGetValue(skillID, out skillVO);
//
//        return skillVO;
//    }

    /// <summary>
    /// 怒气值每一段时间更新一次。时间间隔内只能增加固定量的怒气值。
    /// </summary>
    public void UpdateAnger()
    {
        anger += deltaAnger;
        deltaAnger = 0;
    }

    public void IncreaseAnger(int angerValue)
    {
        deltaAnger += angerValue;
        deltaAnger = Mathf.Clamp(deltaAnger, 0, MAX_DELTA_ANGER);
    }

    public void DecreaseAnger(int angerValue)
    {
        anger -= angerValue;
    }

    public int GetAnger()
    {
        return anger;
    }
    
//    public bool isSkillTargetSelf(int skillID)
//    {
//        SkillVO skillVO = null;
//        skillDic.TryGetValue(skillID, out skillVO);
//        
//        return skillVO != null ? skillVO.Meta.toSelf : false;
//    }
}

