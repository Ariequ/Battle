using System;

/// 技能条件类别
public enum SkillConditionType 
{ 
	///击中目标
	HitTarget, 
	///法术成功释放
	CastSucc,
	///被击中 
	BeHit 
}

/// 技能触发的技能meta
public class SkillLinkMeta
{
	///触发状态ID
	public int id;

    /// 是不是Buff
    public bool isBuff;

	///触发条件
	public SkillConditionType condition;

	///触发几率（从10000中随机）
	public int rate;

	///触发间隔（单位：毫秒）
	public int interval;

	///持续时间（单位：毫秒）
	public float duration;
}

