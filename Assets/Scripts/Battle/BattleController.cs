using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Battle;

public class BattleController : MonoBehaviour 
{
	public const float CELEBRATION_DELAY = 5f;

	public float resultCheckInterval = 3.0f;

	public BattleUIView battleUIView;

	private GameSceneManager sceneDataManager;

	private Transform battleFieldCenter;

	void Awake ()
	{
		this.sceneDataManager = GetComponent<GameSceneManager>();

		ApplicationFacade.Instance.RegisterValueProxy (Faction.Self);
		ApplicationFacade.Instance.RegisterValueProxy (Faction.Opponent);

		ApplicationFacade.Instance.RegisterMediator (new BattleClearingMediator ());
		
		BattleUIMediator battleUIMediator = new BattleUIMediator();
		ApplicationFacade.Instance.RegisterMediator(battleUIMediator);
		sceneDataManager.MessageRoute.RegistryMessageDrivenBehaviour(battleUIMediator);
	}

	void Start () 
	{
		BattleProxy battleProxy = (BattleProxy)ApplicationFacade.Instance.RetrieveProxy (BattleProxy.NAME);
		battleProxy.UpdateFromLevelVO(MockServer.levelVOList[0]);
		battleProxy.UpdateFromLevelVO(MockServer.levelVOList[1], Faction.Self);
		LevelMeta levelMeta = battleProxy.GetCurrentLevel ().Meta;
	
		string prefabPath = ResourcePathConst.GetBattlefieldPrefab (levelMeta.battlefieldID);
		string lightmapBaseName = ResourcePathConst.GetBattlefieldLightmapBaseName (levelMeta.battlefieldID);
		string lightProbesName = ResourcePathConst.GetBattlefieldLightProbes (levelMeta.battlefieldID);

		GameObject prefabObject = ResourceFacade.Instance.LoadPrefab(prefabPath);
		prefabObject.transform.position = Vector3.zero;

		this.battleFieldCenter = prefabObject.transform.FindChild ("Center");
		
		Renderer renderer = prefabObject.GetComponentInChildren<Renderer> ();
		LightmapData lightmapData = new LightmapData();
		lightmapData.lightmapFar = ResourceFacade.Instance.LoadTexture2D(ResourceFacade.PREFAB_PATH + lightmapBaseName + renderer.lightmapIndex.ToString());

		LightmapSettings.lightmaps = new LightmapData[] { lightmapData };
		LightmapSettings.lightProbes = ResourceFacade.Instance.LoadLightProbes (lightProbesName);

		StartCoroutine (StartBattle (battleProxy));
	}

	void OnDestroy () 
	{
		ApplicationFacade.Instance.RemoveValueProxy (Faction.Self);
		ApplicationFacade.Instance.RemoveValueProxy (Faction.Opponent);
		
		ApplicationFacade.Instance.RemoveMediator (BattleClearingMediator.NAME);
		ApplicationFacade.Instance.RemoveMediator(BattleUIMediator.NAME);
	}

	private IEnumerator StartBattle (BattleProxy battleProxy)
	{
		yield return new WaitForSeconds (1.5f);

		List<TroopDefinition> troopDefinitions = new List<TroopDefinition>();
		
		foreach (Faction faction in new Faction[] { Faction.Self, Faction.Opponent }) 
		{
			BattleValueProxy battleValueProxy = ApplicationFacade.Instance.RetrieveValueProxy (faction);
			battleValueProxy.InitializeBattleHeroes (battleProxy.GetHeroVOArray(faction));
			
			SoldierVO[] soldiers = battleProxy.GetSoldierVOArray(faction);

            List<TroopDefinition> troops = new List<TroopDefinition>();
	
			for (int i = 0; i < soldiers.Length; i++)
			{
				TroopDefinition troopDefinition = TroopDefinitionFactory.Singleton.CreateTroopDefinition(i, soldiers[i].Meta, faction);
				troopDefinitions.Add(troopDefinition);
                troops.Add(troopDefinition);
			}

			string magicBehaviorTreeName = faction == Faction.Opponent ? "HeroMagicBehaviorTree" : null;
            SideDefinition m_side = new SideDefinition(troops, battleValueProxy.GetSkillMetaList(), faction, faction.ToString(), magicBehaviorTreeName);
            sceneDataManager.AddSide(m_side);

			yield return new WaitForSeconds (0.5f);
		}

		List<HeroMeta> heros = new List<HeroMeta> ();

		foreach (Faction faction in new Faction[] { Faction.Self, Faction.Opponent }) 
		{
			foreach (HeroVO heroVO in battleProxy.GetHeroVOArray(faction))
			{
				heros.Add(heroVO.Meta);
			}
		}

		sceneDataManager.Initialize ();

		GameElementManager gameElementManager = GetComponent<GameElementManager> ();
		gameElementManager.Initialize (heros, troopDefinitions);

		StartCoroutine (StartGameCheckLoop ());

		ApplicationFacade.Instance.SendNotification(NotificationConst.SHOW_BATTLE_UI, this.battleUIView);
	}

	private IEnumerator StartGameCheckLoop ()
	{ 
		BattleResult battleState = BattleResult.NotOver;

		while (BattleResult.NotOver == battleState)
		{
			yield return new WaitForSeconds (this.resultCheckInterval);
			
            battleState = sceneDataManager.CheckGameState ();
		}

        if (battleState == BattleResult.Win)
        {       
            MockServer.worldVO.defeatedMonsterIDList.Add (MockServer.worldVO.inBattleMonsterID);
            GameObject fireworksEffect = ResourceFacade.Instance.LoadPrefab(PrefabType.Effect, "Effects/Others/FireworksEffect");
			fireworksEffect.transform.position = battleFieldCenter.position;

            yield return new WaitForSeconds(CELEBRATION_DELAY);

            ApplicationFacade.Instance.SendNotification(NotificationConst.BATTLE_COMPLETE, true);
        }
        else if (battleState == BattleResult.Lose)
        {
            ApplicationFacade.Instance.SendNotification(NotificationConst.BATTLE_COMPLETE, false);
        }

	}
}
