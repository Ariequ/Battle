using UnityEngine;
using System.Collections;

public class PrisonMeta 
{
    /// 传送点ID
    public int id;

    public string metaName;

    /// 传送点名称ID
    public int nameID;

    /// 目标地图ID
    public int targetMapID;

    /// 目的地坐标
    public Vector3 targetPosition;

    /// 是否可用
    public bool isAvailable;

    /// 标签
    public string label;
}
