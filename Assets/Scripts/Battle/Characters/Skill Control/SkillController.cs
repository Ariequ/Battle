using System;
using System.Collections.Generic;
using Battle;

public class SkillController : ISkillController
{
    protected Faction targetFaction;
    
    protected SkillVO skillVO;
    protected SkillMeta skillMeta;
    
    protected Vector2 centerPosition;
    
    protected float coreRadius;
    protected float edgeRadius;

    protected BattleAgentManager agentManager;
    protected ISkillContainerDelegate skillContainerDelegate;
    protected BattleValueCalculator valueCalculator;
    
	public virtual void Initialize (BattleAgentManager agentManager, ISkillContainerDelegate skillContainerDelegate, BattleValueCalculator valueCalculator)
    {
        this.agentManager = agentManager;
        this.skillContainerDelegate = skillContainerDelegate;
        this.valueCalculator = valueCalculator;
    }
    
    public virtual void Execute (SkillVO skillVO)
    {
        this.targetFaction = skillVO.Meta.toSelf ? skillVO.casterFaction : FactionTagUtility.GetOppositeFaction(skillVO.casterFaction);
        this.skillMeta = skillVO.Meta;
        this.coreRadius = skillMeta.coreRadius;
        this.edgeRadius = skillMeta.edgeRadius;
        
        this.centerPosition = skillVO.centerPosition;
    }

    ///若范围内目标数量超过上限，则实际效果减少。
    protected float GetReductionRatio(SkillMeta skillMeta, List<BattleAgent> agentList)
    {
        float reductionRatio = 1f;
        
        if (agentList.Count > skillMeta.maxAffectCount)
            reductionRatio *= skillMeta.maxAffectCount * 1f / agentList.Count;

        return reductionRatio;
    }
}
