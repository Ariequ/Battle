using System;
using System.Collections;
using System.Collections.Generic;

public enum SkillType 
{
    Damage,
    Recover,
    Summon,
    Special,
}

public enum PropertyType
{
	Attack = 0,
	Defense,
	HP,
	Luck,
	Dodge,
    None,
}

public class SkillMeta
{
    ///技能ID
    public int id;

    public string metaName;

    ///技能名称ID
    public int nameID;
    ///技能描述ID
    public int descriptionID;

	///技能类型
    public SkillType type;

	///技能分组
    public int group;

	///技能目标
    public bool toSelf;

	///是否AOE
    public bool isAOE;

	///是否随机目标
    public bool isRandom;

	///技能中心范围
    public float coreRadius;
	///技能边缘范围
    public float edgeRadius;

    ///消耗的怒气值
    public int angerExpense;

	///最远有效施放距离
    public float maxDistance;

	///技能图标
    public string iconPath;
	///视觉特效
    public int effectID;
    ///效果延迟
    public float valueDelay;

	///技能影响属性
	public PropertyType propertyType; 
	///技能加成系数
    public float addPercent;
	///技能效果数值最小值
    public int minValue;
	///技能效果数值最大值
    public int maxValue;

	///触发技能列表
	public SkillLinkMeta[] skillLinkMetaArray;

    /// 是否触发额外攻击
    public bool triggerExtraAttack;

    /// 最大影响士兵数量
    public int maxAffectCount;

}

