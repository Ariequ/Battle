using UnityEngine;
using System.Collections;

/// 场景物体类型
public enum MapElementType
{
    Building,
    Monster,
    Treasure,
}

public class MapElementMeta 
{
    /// 物体ID
    public int id;

    public string metaName;

    /// 物体名称ID
    public int nameID;
    /// 物体描述ID
    public int descriptionID;

    /// 物体类型
    public MapElementType type;

    /// 模型路径
    public string prefabPath;

    /// 物体所在地图ID
    public int mapID;

    /// 物体所在坐标
    public Vector3 position;

    /// 特效ID
    public int effectID;

    /// 触发buffID
    public int buffID;

    /// 触发脚本
    public string scriptPath;

    /// 触发传送ID
    public int transportID;
}
