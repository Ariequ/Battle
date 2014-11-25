using System.Collections;
using System.Collections.Generic;
using Battle;

public class ExecuteMagicAction : BTAction
{
    SideAgent agent;
    TroopDefinition definition;

    override public void onInitialize (IAIContext context)
	{
        agent = context.Agent as SideAgent;

        SkillMeta skillMeta = agent.CurrentSkillMeta;
        Faction castFaction = agent.CurrentSkillMeta.toSelf ? agent.faction : FactionUtil.Revert(agent.faction);
        Vector2 castPostion = chooseExecutePlace(skillMeta, findExecuteTroops(skillMeta));


        if (Vector2.IsZero(castPostion))
        {
            m_eStatus = Status.BH_FAILURE;
        }
        else
        {
            SkillVO skillVO = new SkillVO(skillMeta, castFaction);
            skillVO.centerPosition = castPostion;
            agent.skillControllerContainer.ExecuteSkill(skillVO);

            if (skillMeta.metaName == "Resurrection")
            {
                agent.battleAgentManager.RemoveTombStone(definition.name);
//                definition.name = definition.name + "_Resurrection";
                TroopAgent troop = agent.battleAgentManager.AddTroop(definition);
                agent.battleAgentManager.BattleSimulator.AddTroop(troop);
            }

            m_eStatus = Status.BH_SUCCESS;
        }
	}

    private List<TroopAgent> findExecuteTroops (SkillMeta skill)
	{
        Faction castFaction = skill.toSelf ? agent.faction : FactionUtil.Revert(agent.faction);

        return agent.battleAgentManager.GetTroopsByFaction(castFaction);
	}

    private Vector2 chooseExecutePlace (SkillMeta skill, List<TroopAgent> troops)
	{
        if (skill.metaName == "Resurrection")
        {
            List<TroopDefinition> tombStones = agent.battleAgentManager.GetTombStonesByFaction(agent.faction);

            if (tombStones.Count > 0)
            {
                definition = tombStones[0];
                return tombStones[0].position;
            }
            else
            {
                return Vector2.zero;
            }
        }


	    TroopAgent mostSoldierTroop = null;

        foreach (TroopAgent troop in troops) 
		{
            if (mostSoldierTroop == null || troop.Soldiers.Count > mostSoldierTroop.Soldiers.Count)
			{
				mostSoldierTroop = troop;
			}
		}

        if (mostSoldierTroop != null)
        {
            List<BattleAgent> list = new List<BattleAgent>();
            
            foreach( BattleAgent agent in mostSoldierTroop.Soldiers.Values)
            {
                list.Add(agent);        
            }
            
            return list[RandomUtil.Range(0, list.Count)].Position;
        }
        else
        {
            return Vector2.zero;
        }

	}
}
