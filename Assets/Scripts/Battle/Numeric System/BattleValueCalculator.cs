using UnityEngine;
using System;
using System.Collections.Generic;

public enum AttackResult
{
    Fail,
    Normal,
    Critical,
    Dodge }
;

public class BattleValueCalculator
{
    public const float ATTACK_CONSTANT = 1f;
    public const float DEFENSE_CONSTANT = 1f;
    public const int MIN_DAMAGE = 1;
    public const int RANDOM_RANGE = 100;
    public const float CRITICAL_RATIO = 2f;
    public const int EDGE_VALUE_DIVIDER = 2;
    public const int DODGE_BASE = 1000;
    public const int CRITICAL_BASE = 1000;
    private Dictionary<Faction, BattleValueProxy> valueProxyDic;

    public BattleValueCalculator()
    {
        valueProxyDic = new Dictionary<Faction, BattleValueProxy>();
        valueProxyDic[Faction.Self] = ApplicationFacade.Instance.RetrieveValueProxy(Faction.Self);
        valueProxyDic[Faction.Opponent] = ApplicationFacade.Instance.RetrieveValueProxy(Faction.Opponent);
    }

    public AttackResult Attack(AttackValue attackValue, UnitController defenseUnit)
    {
        float dodgeRate = defenseUnit.Meta.dodge * 1f / (defenseUnit.Meta.dodge + DODGE_BASE);
        bool isDodge = RandomUtil.value < dodgeRate;
        if (isDodge)
        {
            return AttackResult.Dodge;
        }

        float critRate = attackValue.luck * 1f / (attackValue.luck + CRITICAL_BASE);
        bool isCrit = RandomUtil.value < critRate;

        int damage;

        if (attackValue.attack > defenseUnit.Defense)
        {
            damage = (int)(((attackValue.attack - defenseUnit.Defense) * (1f + attackValue.damagePercent) + attackValue.damagePoint) / attackValue.maxUnitCount);
        }
        else
        {
            damage = attackValue.level * MIN_DAMAGE;
        }

        if (isCrit)
        {
            damage = (int)(damage * CRITICAL_RATIO) + attackValue.criticalDamage;
        }

        defenseUnit.HP -= damage;

        return isCrit ? AttackResult.Critical : AttackResult.Normal;
    }

    /// <summary>
    /// 计算数值影响，没有逻辑处理
    /// </summary>
    public void CastSkill(SkillMeta skillMeta, UnitController unitController, float reductionRatio, bool inCoreArea = true)
    {
        if (skillMeta.type == SkillType.Damage || skillMeta.type == SkillType.Recover)
        {
			int sign = skillMeta.type == SkillType.Damage ? -1 : 1;

            float randomValue = RandomUtil.Range(skillMeta.minValue, skillMeta.maxValue) * reductionRatio;
            if (inCoreArea)
                randomValue /= EDGE_VALUE_DIVIDER;
			unitController.HP += (int)randomValue * sign;
        }

    }

    /// <summary>
    /// 士兵释放的技能。也是普通伤害类型。
    /// </summary>
    public void CastSoldierSkill(UnitController attackUnit, List<UnitController> unitList)
    {
    }

}

