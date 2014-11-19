using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;

public class BattleProxy : Proxy
{
	new public const string NAME = "BattleProxy";

	public const int MAX_HERO_COUNT = 3;

	public const int MAX_SOLDIER_COUNT = 5;

	private LevelVO _currentLevel;

	//Heroes you select to attend the battle.
	private Dictionary<Faction, HeroVO[]> _heroVOListDic = new Dictionary<Faction, HeroVO[]>();
	
	//The soldiers you arrange for the battle.
	private Dictionary<Faction, SoldierVO[]> _soldierVOListDic = new Dictionary<Faction, SoldierVO[]>();

	public BattleProxy () : base(NAME)
	{
		InitializeData ();
	}

	private void InitializeData ()
	{
		_heroVOListDic.Add(Faction.Self,		new HeroVO[MAX_HERO_COUNT]);
		_heroVOListDic.Add(Faction.Opponent,	new HeroVO[MAX_HERO_COUNT]);

		_soldierVOListDic.Add(Faction.Self,		new SoldierVO[MAX_SOLDIER_COUNT]);
		_soldierVOListDic.Add(Faction.Opponent,	new SoldierVO[MAX_SOLDIER_COUNT]);
	}

	public override void OnRemove ()
	{
		_heroVOListDic.Clear();

		_soldierVOListDic.Clear();
	}

	public void UpdateFromLevelVO(LevelVO levelVO, Faction faction = Faction.Opponent)
	{
		this._currentLevel = levelVO;

		LevelMeta levelMeta = levelVO.Meta;
		SoldierVO[] soldierVOList = new SoldierVO[levelMeta.enemySoldierIDs.Length];

		for (int i = 0; i < soldierVOList.Length; ++i)
		{
			int soldierID = levelMeta.enemySoldierIDs[i];
			soldierVOList[i] = new SoldierVO(MetaManager.Instance.GetSoldierMeta(soldierID), Faction.Opponent);
		}

		SetSoldierVOArray(faction, soldierVOList);

		HeroVO[] heroVOList = new HeroVO[levelMeta.enemyHeroIDs.Length];

		for (int i = 0; i < heroVOList.Length; ++i)
		{
			int heroID = levelMeta.enemyHeroIDs[i];
			HeroVO heroVO = new HeroVO(MetaManager.Instance.GetHeroMeta(heroID), Faction.Opponent);
			heroVO.level = levelMeta.enemyHeroLevels[i];
			heroVO.starLevel = levelMeta.enemyHeroStarLevels[i];
			heroVOList[i] = heroVO;
		}

		SetHeroVOArray (faction, heroVOList);
	}

	public LevelVO GetCurrentLevel ()
	{
		return this._currentLevel;
	}

	public HeroVO[] GetHeroVOArray(Faction faction)
	{
		HeroVO[] heroVOArray = new HeroVO[MAX_HERO_COUNT];
		_heroVOListDic[faction].CopyTo(heroVOArray, 0);

		return heroVOArray;
	}

	public void SetHeroVOArray(Faction faction, HeroVO[] heroVOList)
	{
		HeroVO[] heroVOArray = _heroVOListDic[faction];

		for (int i = 0; i < heroVOArray.Length; i++)
		{
			heroVOArray[i] = i < heroVOList.Length ? heroVOList[i] : null;
		}
	}

	public SoldierVO[] GetSoldierVOArray(Faction faction)
	{
		SoldierVO[] soldierVOArray = new SoldierVO[MAX_SOLDIER_COUNT];
		_soldierVOListDic[faction].CopyTo(soldierVOArray, 0);

		return soldierVOArray;
	}

	public void SetSoldierVOArray(Faction faction, SoldierVO[] soldierVOList)
	{
		SoldierVO[] soldierVOArray = _soldierVOListDic[faction];

		for (int i = 0; i < soldierVOArray.Length; i++)
		{
			soldierVOArray[i] = i < soldierVOList.Length ? soldierVOList[i] : null;
		}
	}

	public bool ReadyForStart ()
	{
		HeroVO[] heroVOArray = _heroVOListDic [Faction.Self];
		SoldierVO[] soldierVOArray = _soldierVOListDic [Faction.Self];

		int heroCount = 0;
		int soldierCount = 0;

		foreach (HeroVO hero in heroVOArray)
		{
			if (hero != null) heroCount ++;
		}

		foreach (SoldierVO soldier in soldierVOArray)
		{
			if (soldier != null) soldierCount ++;
		}

		return heroCount > 0 && soldierCount > 0;
	}
}

