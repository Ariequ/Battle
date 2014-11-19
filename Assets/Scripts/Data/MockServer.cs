using System;
using System.Collections.Generic;
using UnityEngine;

public class MockServer
{
    public static bool isMocking = true;

    public static bool initialized = false;

//    public static LordVO lordVO;

//    public static LegendVO worldVO;

	public static List<HeroVO> heroVOList = new List<HeroVO>();
	public static List<SoldierVO> soldierVOList = new List<SoldierVO>();
	public static List<LevelVO> levelVOList = new List<LevelVO>();

	public static void Initialize ()
	{
        if (!initialized)
        {
//            lordVO = new LordVO();
//            lordVO.lordName = "Tokugawa";
//            lordVO.energy = 100;
//            lordVO.level = 3;
//            lordVO.exp = 0;
//            lordVO.gem = 999;
//            lordVO.gold = 40000;
//            lordVO.wood = 5000;
//            lordVO.ore = 5000;
//            lordVO.vipLevel = 3;

    		for (int i = 1; i <= 8; i++)
    		{
    			HeroMeta heroMeta = MetaManager.Instance.GetHeroMeta (i);
    			HeroVO heroVO = new HeroVO (heroMeta, Faction.Self);
    			heroVOList.Add(heroVO);
    		}

    		for (int i = 1; i <= 12; i++)
    		{
    			SoldierMeta soldierMeta = MetaManager.Instance.GetSoldierMeta (i);
    			SoldierVO soldierVO = new SoldierVO (soldierMeta, Faction.Self);
    			soldierVOList.Add(soldierVO);
    		}

    		for (int i = 1; i <= 2; i++)
    		{
    			LevelMeta levelMeta = MetaManager.Instance.GetLevelMeta (i);
    			LevelVO levelVO = new LevelVO (levelMeta);
    			levelVOList.Add(levelVO);
    		}

//            worldVO = new LegendVO();
//			worldVO.id = 3;
//            worldVO.lordPosition = new Vector3();
//            worldVO.inBattleMonsterID = -1;
//            worldVO.defeatedMonsterIDList = new List<int>();
//            worldVO.defeatedTreasureIDList = new List<int>();

            initialized = true;
        }
	}

    public static int GetParliamentData()
    {
        return 400;
    }

    public static int GetTax()
    {
        return 400;
    }

//	public static void RecordLordTransform (Transform lordTransform)
//	{
//		worldVO.lordPosition = lordTransform.position;
//		worldVO.lordRotation = lordTransform.rotation;
//	}
//
//	public static void LoadLordTransform (Transform lordTransform, Vector3 defaultPosition, Quaternion defaultRotation)
//	{
//		lordTransform.position = Vector3.zero != worldVO.lordPosition ? worldVO.lordPosition : defaultPosition;
//		lordTransform.rotation = Quaternion.identity != worldVO.lordRotation ? worldVO.lordRotation : defaultRotation;
//	}

    public static Dictionary<string, object> GetBuildingUpgradeData(int buildingMetaID)
    {
        CityBuildingMeta buildingMeta = MetaManager.Instance.GetCityBuildingMeta(buildingMetaID);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data[UpgradeProxy.IS_UPGRADING] = false;
        data[UpgradeProxy.REMAIN_TIME] = buildingMeta.upgradeDuration;
        return data;
    }

    public static Dictionary<string, object> UpgradeBuilding(int buildingMetaID)
    {
        CityBuildingMeta buildingMeta = MetaManager.Instance.GetCityBuildingMeta(buildingMetaID);
        int nextMetaID = buildingMeta.upgradeBuildingID;
        long duration = buildingMeta.upgradeDuration;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data[UpgradeProxy.META_ID] = nextMetaID;
        data[UpgradeProxy.IS_UPGRADING] = true;
        data[UpgradeProxy.REMAIN_TIME] = duration;
        return data;
    }
}

