using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using Battle;

public class SideAgent :IAIAgent, IAIContext 
{
	private Behavior behavior;

    public Dictionary<string, TroopAgent> troopDictionary = new Dictionary<string, TroopAgent>();
    public Faction faction;
    public List<SkillMeta> skillMetaList;
    public string sideName;
    public BattleAgentManager battleAgentManager;
    public SkillControllerContainer skillControllerContainer;

    public SkillMeta CurrentSkillMeta;

    private Behavior m_Behavior;

    public SideAgent(List<SkillMeta> skillMetaList, Faction faction, string sideName,BattleAgentManager battleAgentManager, XmlDocument behaviorTree, SkillControllerContainer skillControllerContainer = null)
    {
        this.faction = faction;
        this.sideName = sideName;
        this.skillMetaList = skillMetaList;
        this.battleAgentManager = battleAgentManager;
        this.skillControllerContainer = skillControllerContainer;

		if (behaviorTree != null)
		{
			this.m_Behavior = BehaviorTreeParser.parseBehaviorTree(behaviorTree.FirstChild);
		}
    }

    public void AddTroop(TroopAgent troop)
    {
        troopDictionary.Add(troop.Name, troop);
    }

    public void RemoveTroop(string troopName)
    {
        troopDictionary.Remove(troopName);
    }
    
    public void Tick()
    {
		if (this.m_Behavior != null)
		{
			m_Behavior.tick(this);
		}
    }

    public object Agent
    {
        get
        {
            return this;
        }
    }
}
