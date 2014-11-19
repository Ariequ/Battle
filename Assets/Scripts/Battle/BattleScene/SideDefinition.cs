using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Battle;

public class SideDefinition
{
    public List<TroopDefinition> troops;
    public Faction faction;
    public List<SkillMeta> skillMetas;
    public string sideName;
    public string behaviorTreeName;

	public SideDefinition(List<TroopDefinition> troops, List<SkillMeta> skillMetas, Faction faction, string sideName, string behaviorTreeName = null)
    {
        this.troops = troops;
        this.faction = faction;
        this.skillMetas = skillMetas;
        this.sideName = sideName;
		this.behaviorTreeName = behaviorTreeName;
    }
}
