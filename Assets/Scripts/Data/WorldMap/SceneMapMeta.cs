using UnityEngine;

using System;

/// 地图类型
public enum SceneMapType
{
}

public class SceneMapMeta
{
	/// 地图ID
	public int id;

	public string metaName;

	/// 地图名称ID
	public int nameID;
	/// 地图描述ID
	public int descriptionID;

	/// 地图类型
	public SceneMapType type;

    /// 是否可用
    public bool isAvailable;

	/// 标签
	public string label;

	/// 默认坐标
	public Vector3 defaultPosition;
}

