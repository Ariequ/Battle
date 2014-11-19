using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Battle;

public class TestGameSceneInitialization : MonoBehaviour 
{
    public bool showHPLabel = false;
    public bool showMagicLabel = false;
    public bool showTroopLabel = true;

    public int[] selfSoldiers;

    public int[] opponentSoldiers;

    private MockData mockData = new MockData();

    void Start ()
    {
        StartCoroutine (StartBattle());
    }

    private IEnumerator StartBattle ()
    {
        yield return new WaitForSeconds (0.5f);

        MetaManager.InitializeSingleton(mockData);

        List<TroopDefinition> troopDefinitions = new List<TroopDefinition>();
        GameSceneManager sceneDataManager = GetComponent<GameSceneManager>();

        LevelMeta levelMeta = MetaManager.Instance.GetLevelMeta(1);

        List<TroopDefinition> m_troops = new List<TroopDefinition>();

        for (int i = 0; i < selfSoldiers.Length; i++)
        {
            SoldierMeta soldierMeta = MetaManager.Instance.GetSoldierMeta(selfSoldiers[i]);
            TroopDefinition troopDefinition = TroopDefinitionFactory.Singleton.CreateTroopDefinition(i, soldierMeta, Faction.Self);
//          sceneDataManager.AddTroop(troopDefinition);
            troopDefinitions.Add(troopDefinition);
            m_troops.Add(troopDefinition);
        }

        SideDefinition m_side = new SideDefinition(m_troops, null, Faction.Self, Faction.Self.ToString());
        sceneDataManager.AddSide(m_side);
                               

        List<TroopDefinition> o_troops = new List<TroopDefinition>();
        for (int i = 0; i < opponentSoldiers.Length; i++)
        {
            SoldierMeta soldierMeta = MetaManager.Instance.GetSoldierMeta(opponentSoldiers[i]);
            TroopDefinition troopDefinition = TroopDefinitionFactory.Singleton.CreateTroopDefinition(i, soldierMeta, Faction.Opponent);
//          sceneDataManager.AddTroop(troopDefinition);
            troopDefinitions.Add(troopDefinition);
            o_troops.Add(troopDefinition);
        }

        List<SkillMeta> skillMetas = new List<SkillMeta>();

        for(int i = 0; i < levelMeta.enemyHeroIDs.Length; i++)
        {
            HeroMeta meta = MetaManager.Instance.GetHeroMeta(levelMeta.enemyHeroIDs[i]);
            SkillMeta skillMeta = MetaManager.Instance.GetSkillMeta(meta.skillDic[levelMeta.enemyHeroStarLevels[i]]);
            skillMetas.Add(skillMeta);
        }

        SideDefinition o_side = new SideDefinition(o_troops, skillMetas, Faction.Opponent, Faction.Opponent.ToString(), "HeroMagicBehaviorTree");
        sceneDataManager.AddSide(o_side);
        
        GameElementManager gameElementManager = GetComponent<GameElementManager> ();
        gameElementManager.Initialize (null, troopDefinitions);
    }
}
