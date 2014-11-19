using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;

public class SkillControllerContainer : ISkillContainerDelegate
{
	private Dictionary<SkillType, Type> skillLogicTypeDic;

    protected BattleAgentManager agentManager;
    private BattleValueCalculator valueCalculator;
	private GameMessageRoute outerMessageRoute;

	public SkillControllerContainer (BattleAgentManager agentManager, GameMessageRoute outerMessageRoute)
	{
        this.agentManager = agentManager;
        this.valueCalculator = agentManager.ValueCalculator;
		this.outerMessageRoute = outerMessageRoute;

        skillLogicTypeDic = new Dictionary<SkillType, Type>();
        skillLogicTypeDic.Add(SkillType.Damage, typeof(DamageSkillController));
        skillLogicTypeDic.Add(SkillType.Recover, typeof(RecoverSkillController));
	}

    public void ExecuteSkill(SkillVO skillVO)
    {
        SkillType skillType = skillVO.Meta.type;

        Type skillLogicType = skillLogicTypeDic[skillType];

        ISkillController skillController = Activator.CreateInstance(skillLogicType) as ISkillController;
        skillController.Initialize(agentManager, this, valueCalculator);

        skillController.Execute(skillVO);

        Dictionary<String, System.Object> data = new Dictionary<String, System.Object> ();
        data.Add(MessageParamKey.METAID, skillVO.Meta.id);
        data.Add(MessageParamKey.FACTION, skillVO.casterFaction);
		data.Add(MessageParamKey.POSITION, skillVO.centerPosition);

		outerMessageRoute.SendGameMessage(new MessageContext(Messages.SHOW_SKILL_EFFECT, 0, data));
    }

    public void ExecuteSkillsBehind(UnitController unitController, Battle.Vector2 position, SkillLinkMeta[] skillLinkMetaArray)
    {
        foreach (SkillLinkMeta linkMeta in skillLinkMetaArray)
        {
            if (linkMeta.isBuff)
            {
                BuffMeta linkedBuffMeta = MetaManager.Instance.GetBuffMeta(linkMeta.id);
                unitController.AddBuff(linkedBuffMeta);
            }
            else
            {
                SkillMeta linkedSkillMeta = MetaManager.Instance.GetSkillMeta(linkMeta.id); 
				SkillVO skillVO = new SkillVO(linkedSkillMeta, linkedSkillMeta.toSelf ? unitController.Faction : FactionTagUtility.GetOppositeFaction(unitController.Faction));
                skillVO.centerPosition = position;
                ExecuteSkill(skillVO);
            }
        }
    }
}

