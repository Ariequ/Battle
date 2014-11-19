using System;
using System.Collections;

/// 士兵（怪物）体型类别
public enum SizeLevel {Small, Medium, Large}

public class SoldierMeta 
{
    ///兵种ID
    public int id;

    public string metaName;

    ///兵种名称ID
    public int nameID;
    ///兵种描述ID
    public int descriptionID;

	///Prefab Path
	public string prefabPath;

    ///兵种体型
    public SizeLevel sizeLevel;
    ///出战总量上限
    public int maxUnitCount;

    /// 士兵体型半径
    public float boundsRadius;

    /// 士兵所在物理层
    public int layer;

    ///初始等级
    public int initialLevel;
    ///等级上限
    public int maxLevel;

    /// 势力
    public Force force;

    ///初始攻击值
    public int initialAttack;
    ///攻击成长值
    public float attackIncrement; 

    ///初始防御值
    public int initialDefense;
    ///防御成长值
    public float defenseIncrement;

    ///初始生命值
    public int initialHP;
    ///生命成长值
    public float hpIncrement;

	///初始幸运值（从10000中随机）
    public int initialLuck;
	///幸运成长值（从10000中随机）
    public float luckIncrement;

	///闪避（从10000中随机）
    public int dodge;

	///攻击频率（单位：毫秒）
    public int attackFrequency;
    ///最近攻击距离
    public int attackMinRange;
    /// 最远攻击距离
    public int attackMaxRange;
    ///暴击伤害
    public int criticalDamage;
    ///伤害数值加成
    public int damagePoint;
    ///伤害百分比加成
    public float damagePercent;

    ///技能攻击id
    public int skillID;
	///技能攻击几率（从10000中随机）
    public int skillRate;
	///技能攻击冷却（单位：毫秒）
    public int skillCD;

    ///训练消耗金钱
    public Price consumptionCost;
	///训练消耗时间（单位：毫秒）
    public DateTime consumptionDuration;

    ///模型资源
    public string modelPath;
    ///头像资源
    public string iconPath;

    ///死亡消耗金币
    public Price deathConsumptionCost;
    ///升级消耗金币
    public Price upgradeConsumptionCost;
    ///进阶消耗金币
    public Price promoteConsumptionCost;

    ///分解产生灵魂最小值
    public int decomposeSoulMin;
    ///分解产生灵魂最大值
    public int decomposeSoulMax;

    /// 子弹的元数据
    public BulletMeta bulletMeta;

}
