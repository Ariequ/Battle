using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Battle;

public class RecoverSkillController : SkillController
{
	public override void Execute(SkillVO skillVO)
    {
        base.Execute(skillVO);
        
        List<BattleAgent> resultList = agentManager.SearchAroundPoint(skillVO.centerPosition, coreRadius, skillVO.casterFaction);

        float reductionRatio = GetReductionRatio(skillMeta, resultList);

        if (skillMeta.isAOE)
        {
            foreach (BattleAgent agent in resultList)
            {
                valueCalculator.CastSkill(skillMeta, agent.UnitController, reductionRatio);
                skillContainerDelegate.ExecuteSkillsBehind(agent.UnitController, skillVO.centerPosition, skillMeta.skillLinkMetaArray);
            }
        }
        else if (resultList.Count > 0)
        {
            UnitController unitController = resultList[0].UnitController;
            valueCalculator.CastSkill(skillVO.Meta, unitController, 1f);
            
            skillContainerDelegate.ExecuteSkillsBehind(unitController, skillVO.centerPosition, skillMeta.skillLinkMetaArray);
        }
    }
}

