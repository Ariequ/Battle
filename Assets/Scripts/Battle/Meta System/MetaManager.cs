using System;
using System.Collections.Generic;
using UnityEngine;

public class MetaManager
{
	private Dictionary<int, EffectMeta> effectMetaDic;
    private Dictionary<int, BulletMeta> bulletMetaDic;
    private Dictionary<int, SoldierMeta> soldierMetaDic;
    private Dictionary<int, HeroMeta> heroMetaDic;
    private Dictionary<int, SkillMeta> skillMetaDic;
    private Dictionary<int, BuffMeta> buffMetaDic;
    private Dictionary<int, CityBuildingMeta> cityBuildingMetaDic;
    private Dictionary<int, LevelMeta> levelMetaDic;
    private Dictionary<int, MapElementMeta> mapBuildingMetaDic;
    private Dictionary<int, MapElementMeta> mapMonsterMetaDic;
    private Dictionary<int, MapElementMeta> mapTreasureMetaDic;
    private Dictionary<int, long> lordLevelExpDic;
    private static MetaManager instance;

    public static void InitializeSingleton(MockData mockData)
    {
        if (instance == null)
        {
            MetaManager metaManager = new MetaManager();

			metaManager.effectMetaDic = mockData.effectMetaDic;
			metaManager.bulletMetaDic = mockData.bulletMetaDic;
            metaManager.soldierMetaDic = mockData.soldierMetaDic;
            metaManager.heroMetaDic = mockData.heroMetaDic;
            metaManager.skillMetaDic = mockData.skillMetaDic;
            metaManager.buffMetaDic = mockData.buffMetaDic;
            metaManager.cityBuildingMetaDic = mockData.cityBuildingMetaDic;
            metaManager.levelMetaDic = mockData.levelMetaDic;
            metaManager.mapBuildingMetaDic = mockData.mapBuildingMetaDic;
            metaManager.mapMonsterMetaDic = mockData.mapMonsterMetaDic;
            metaManager.lordLevelExpDic = mockData.lordLevelExpDic;
            metaManager.mapTreasureMetaDic = mockData.mapTreasureMetaDic;

            instance = metaManager;
        }
    }

    public static MetaManager Instance
    {
        get
        {
            return instance;
        }
    }

    private MetaManager()
    {

    }

	public EffectMeta GetEffectMeta(int metaID)
	{
		return effectMetaDic[metaID];
	}

    public BulletMeta GetBulletMeta(int metaID)
    {
        return bulletMetaDic[metaID];
    }

    public SoldierMeta GetSoldierMeta(int metaID)
    {
        return soldierMetaDic[metaID];
    }

    public HeroMeta GetHeroMeta(int metaID)
    {
		if (heroMetaDic.ContainsKey (metaID))
			return heroMetaDic [metaID];
		else
			Debug.LogError ("HeroMetaID:"+metaID+" not found!");

				return null;
    }

    public SkillMeta GetSkillMeta(int metaID)
    {
        return skillMetaDic[metaID];
    }

    public BuffMeta GetBuffMeta(int metaID)
    {
        return buffMetaDic[metaID];
    }

    public CityBuildingMeta GetCityBuildingMeta(int metaID)
    {
        if (cityBuildingMetaDic.ContainsKey(metaID))
        {
            return cityBuildingMetaDic[metaID];
        }
        else
        {
            Debug.Log("No Building Meta of ID " + metaID);
           
            return null;
        }
    }

    public LevelMeta GetLevelMeta(int metaID)
    {
        return levelMetaDic[metaID];
    }

    public MapElementMeta GetMapElementMeta(int metaID, MapElementType type)
    {
        //TODO: Get the soldier meta directly and load the model by another path property.
        switch (type)
        {
            case MapElementType.Building:
                return mapBuildingMetaDic[metaID];
            case MapElementType.Monster:
                return mapMonsterMetaDic[metaID];
            case MapElementType.Treasure:
                return mapTreasureMetaDic[metaID];
            default:
                return null;
        }

    }

    public long GetLordLevelExp(int level)
    {
        return lordLevelExpDic[level];
    }
}

