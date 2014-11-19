using UnityEngine;
using System;
using System.Collections;

/// 建筑类型
public enum CityBuildingType
{
    Parliament,
    Defense,
    Goldmine,
    Smithy
}

public class CityBuildingMeta
{
    /// 建筑ID
    public int id;

	public string metaName;

    /// 建筑名称ID
    public int nameID;
    /// 建筑描述ID
    public int descriptionID;

	/// 建筑类型
	public CityBuildingType type;

    /// 建筑图标
	public string iconPath;

    /// 触发buff
	public int buffID;

    /// 购买价格
	public Price buildPrice;

    /// 升级价格
	public Price upgradePrice;

	/// 建造时间（单位：秒）
	public long buildDuration;

	/// 升级时间（单位：秒）
	public long upgradeDuration;

    /// 升级目标建筑
    public int upgradeBuildingID;

    /// 生产货币类型
    public CurrencyType productType;

    /// 生产货币数量
    public int productAmount;

    /// 生产/招募兵种ID
    public int produceSoldierID;

    /// 一次生产/招募兵种数量
    public int produceSoldierAmount;

}
