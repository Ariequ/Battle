using System;
using System.Collections;

/// 道具类型
public enum PropType {}

public class PropMeta
{
    /// 道具ID
    public int id;

    public string metaName;

    /// 道具名称ID
    public int nameID;
    /// 道具描述ID
    public int descriptionID;

    /// 道具类型
    public PropType type;

    /// 道具图标
    public string iconPath;

    /// 堆叠上限
    public int stackLimit;

    /// 购买价格
    public Price buyPrice;

    /// 出售价格
    public Price sellPrice;

    /// 获得经验
	public long gainExp;

    /// 获得资源
    public Price gainResource;
}
