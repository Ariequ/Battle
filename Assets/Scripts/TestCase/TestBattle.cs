using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestBattle : MonoBehaviour
{
	public Canvas canvas;

	public void StartBattle()
	{
		ApplicationFacade.Instance.SendNotification (NotificationConst.BATTLE_PREPARATION_SHOW, MockServer.levelVOList [0]);
//
//		MockServer.worldVO.inBattleMonsterID = 1;
//		BattleProxy battleProxy = ApplicationFacade.Instance.RetrieveProxy(BattleProxy.NAME) as BattleProxy;
//		battleProxy.UpdateFromLevelVO(MockServer.levelVOList [0]);
//		UpdateFromLevelVO(battleProxy, MockServer.levelVOList [1]);
//
//		GameGlobal.LoadSceneByName(SceneNameConst.BATTLE_SCENE);
	}

	private void UpdateFromLevelVO(BattleProxy battleProxy, LevelVO levelVO)
	{
		LevelMeta levelMeta = levelVO.Meta;
		SoldierVO[] soldierVOList = new SoldierVO[levelMeta.enemySoldierIDs.Length];
			
		for (int i = 0; i < soldierVOList.Length; ++i)
		{
			int soldierID = levelMeta.enemySoldierIDs [i];
			soldierVOList [i] = new SoldierVO (MetaManager.Instance.GetSoldierMeta(soldierID), Faction.Opponent);
		}
			
		battleProxy.SetSoldierVOArray(Faction.Self, soldierVOList);
			
		HeroVO[] heroVOList = new HeroVO[levelMeta.enemyHeroIDs.Length];
			
		for (int i = 0; i < heroVOList.Length; ++i)
		{
			int heroID = levelMeta.enemyHeroIDs [i];
			HeroVO heroVO = new HeroVO (MetaManager.Instance.GetHeroMeta(heroID), Faction.Opponent);
			heroVO.level = levelMeta.enemyHeroLevels [i];
			heroVO.starLevel = levelMeta.enemyHeroStarLevels [i];
			heroVOList [i] = heroVO;
		}
			
		battleProxy.SetHeroVOArray(Faction.Self, heroVOList);
	}

	private void hideView()
	{
		canvas.gameObject.SetActive(false);
	}
}
