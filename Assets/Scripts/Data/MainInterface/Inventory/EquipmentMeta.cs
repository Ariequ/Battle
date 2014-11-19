using System;

/// 装备类型
public enum EquipmentType
{
}

public class EquipmentMeta
{
	/// 装备ID
	public int id;

	public string metaName;

	/// 装备名称ID
	public int nameID;
	/// 装备描述ID
	public int descriptionID;

	/// 装备类型
	public EquipmentType type;

	/// 装备品质
	public int quality;

	/// 初始攻击值
	public int initialAttack;
	/// 攻击成长值
	public float attackIncrement; 

	/// 初始防御值
	public int initialDefense;
	/// 防御成长值
	public float defenseIncrement;

	/// 初始生命值
	public int initialHP;
	/// 生命成长值
	public float hpIncrement;

	/// 初始幸运值（从10000中随机）
	public int initialLuck;
	/// 幸运成长值（从10000中随机）
	public float luckIncrement;

	/// 特殊状态
	public int specialState;

	/// 专精加成百分比
	public int masteryAddPercent;

	/// 装备图标
	public string iconPath;

	/// 堆叠上限
	public int stackLimit;

	/// 购买价格
	public Price buyPrice;

	/// 出售价格
	public Price sellPrice;

    ///装备所带魔法
    public int magicID;
}

