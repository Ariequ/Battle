using System;
using System.Collections;
using System.Collections.Generic;

/// 英雄类型
using UnityEngine;


public enum HeroType
{

}

/// 英雄品质
public enum HeroQuality
{
	White = 1,
	Green,
	Blue,
	Purple1,
	Purple2,
	Purple3,
	Orange1,
	Orange2,
}

public class HeroMeta
{
	public Color[] color = {
		new Color (1,1,1),
		new Color (0,1,0.28f),
		new Color (0,0.41f,1),
		new Color (0.57f,0.25f,1),
		new Color (0.57f,0.25f,1),
		new Color (0.57f,0.25f,1),
		new Color (1,0.52f,0.25f),
		new Color (1,0.52f,0.25f)} ;

    public const int MAX_STAR_LEVEL = 5;

	/// 英雄ID
	public int id;

	public string metaName;

	/// 英雄名称ID
	public int nameID;
	/// 英雄描述ID
	public int descriptionID;

	///英雄类型
	public HeroType type;

    /// 势力
    public Force force;

	///英雄分组
	public int group;

	///英雄品质
	public HeroQuality quality;

	///英雄星级
	public int starLevel;
	///英雄初始星级
	public int initialStarLevel;

	///星级技能
    public Dictionary<int, int> skillDic;

	///初始等级
	public int initialLevel;

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

	///死亡消耗金币
	public Price deathConsumptionCost;
	///升级消耗金币
	public Price upgradeConsumptionCost;
	///进阶消耗金币
	public Price promoteConsumptionCost;

	///模型资源
	public string modelPath;
	///头像资源
	public string iconPath;

	///分解产生灵魂最小值
	public int decomposeSoulMin;
	///分解产生灵魂最大值
	public int decomposeSoulMax;
}
