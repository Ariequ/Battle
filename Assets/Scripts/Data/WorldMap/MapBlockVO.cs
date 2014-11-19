using UnityEngine;
using System;
using System.Collections.Generic;

public class MapBlockVO
{
	public GameObject gameObject;

    private MapBlockJsonData jsonData;
    public MapBlockJsonData JsonData 
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

                float x = Convert.ToSingle(jsonData.x);
                float y = Convert.ToSingle(jsonData.y);
                float z = Convert.ToSingle(jsonData.z);
                position = new Vector3(x, y, z);

                float width = Convert.ToSingle(JsonData.width);
                float height = Convert.ToSingle(JsonData.height);
                float length = Convert.ToSingle(JsonData.length);
                extent = new Vector3(width, height, length);


				indexList = jsonData.indexList;
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

    private Vector3 position;
    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    private Vector3 extent;
    public Vector3 Extent
    {
        get
        {
            return extent;
        }
    }

	private int[] indexList;

	/// <summary>
	/// 光照贴图索引
	/// </summary>
	/// <value>The index list.</value>
	public int[] IndexList
	{
		get
		{
			return indexList;
		}
	}
}

