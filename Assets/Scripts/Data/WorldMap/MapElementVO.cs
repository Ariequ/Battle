using UnityEngine;
using System;

public enum MapElementState
{
    state1,
    state2
}

public class MapElementVO
{
    public MapElementMeta meta;

    public GameObject gameObject;

    private MapElementJsonData jsonData;
    public MapElementJsonData JsonData 
    { 
        private get
        {
            return jsonData;
        }
        set
        {
            jsonData = value;
            if (jsonData != null)
            {
                id = Convert.ToInt32(jsonData.id);
                int metaID = Convert.ToInt32(jsonData.metaID);

                type = (MapElementType)Enum.Parse(typeof(MapElementType), jsonData.type);

                meta = MetaManager.Instance.GetMapElementMeta(metaID, type);

                float x = Convert.ToSingle(jsonData.localX);
                float y = Convert.ToSingle(jsonData.localY);
                float z = Convert.ToSingle(jsonData.localZ);
                position = new Vector3(x, y, z);

				float rotationX = Convert.ToSingle(jsonData.rotationX);
				float rotationY = Convert.ToSingle(jsonData.rotationY);
				float rotationZ = Convert.ToSingle(jsonData.rotationZ);
				rotation = new Vector3(rotationX, rotationY, rotationZ);

				lightmapIndex = Convert.ToInt32(jsonData.lightmappingIndex);

				float lightX = Convert.ToSingle(jsonData.lightX);
				float lightY = Convert.ToSingle(jsonData.lightY);
				float lightZ = Convert.ToSingle(jsonData.lightZ);
				float lightW = Convert.ToSingle(jsonData.lightW);
				lightmapTilingOffset = new Vector4(lightX, lightY, lightZ, lightW);
			}
            else
            {
                Debug.LogError("Wrong json data!");
            }
        }
    }

    private int id;
    public int ID
    {
        get
        {
            return id;
        }
    }

    private MapElementType type;
    public MapElementType ElementType
    {
        get
        {
            return type;
        }
    }
    
    public string Path
    {
        get
        {
            return meta.prefabPath;
        }
    }
    
    private Vector3 position = Vector3.zero;
    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

	private Vector3 rotation = Vector3.zero;
	public Vector3 Rotation
	{
		get
		{
			return rotation;
		}
	}

	private int lightmapIndex;
	public int LightmapIndex
	{
		get
		{
			return lightmapIndex;
		}
	}

	private Vector4 lightmapTilingOffset;
	public Vector4 LightmapTilingOffset
	{
		get
		{
			return lightmapTilingOffset;
		}
	}

    public MapElementState state;
}

