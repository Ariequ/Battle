using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Battle;

public class DamageSkillController : SkillController
{
    public override void Execute(SkillVO skillVO)
    {
        base.Execute(skillVO);

        List<BattleAgent> wholeResultList = agentManager.SearchAroundPoint(skillVO.centerPosition, this.edgeRadius, this.targetFaction);

        if (skillMeta.isAOE)
        {
            float reductionRatio = GetReductionRatio(skillMeta, wholeResultList);

            List<UnitController> coreUnitList = new List<UnitController>();
            List<UnitController> edgeUnitList = new List<UnitController>();

            foreach (BattleAgent agent in wholeResultList)
            {
                UnitController unitController = agent.UnitController;
                if (Battle.Vector2.Distance(agent.Position, skillVO.centerPosition) < coreRadius)
                {
                    coreUnitList.Add(unitController);
                    valueCalculator.CastSkill(skillVO.Meta, unitController, reductionRatio);
                }
                else
                {
                    edgeUnitList.Add(unitController);
                    valueCalculator.CastSkill(skillVO.Meta, unitController, reductionRatio, false);
                }

				if (skillMeta.skillLinkMetaArray != null)
				{
                	skillContainerDelegate.ExecuteSkillsBehind(unitController, skillVO.centerPosition, skillMeta.skillLinkMetaArray);
				}
            }
        }
        else if (wholeResultList.Count > 0)
        {
            UnitController unitController = wholeResultList[0].UnitController;
            valueCalculator.CastSkill(skillVO.Meta, unitController, 1f);

			if (skillMeta.skillLinkMetaArray != null)
			{
            	skillContainerDelegate.ExecuteSkillsBehind(unitController, skillVO.centerPosition, skillMeta.skillLinkMetaArray);
			}
        }
    }
}

