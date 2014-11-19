using UnityEngine;

using System;
using System.Collections.Generic;

public class MapGlobalVO
{
	public int maxLightmapIndex;
	
	public Vector3 lordPosition;
	public Vector3 lordRotation;
	
	public Dictionary<string, Dictionary<string, string>> scriptsInfoDic;

	private MapGlobalJsonData jsonData;
	public MapGlobalJsonData JsonData 
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
				maxLightmapIndex = Convert.ToInt32(jsonData.maxLightmapIndex);
				
				float x = Convert.ToSingle(jsonData.lordX);
				float y = Convert.ToSingle(jsonData.lordY);
				float z = Convert.ToSingle(jsonData.lordZ);
				lordPosition = new Vector3(x, y, z);

				float rotationX = Convert.ToSingle(jsonData.lordRotationX);
				float rotationY = Convert.ToSingle(jsonData.lordRotationY);
				float rotationZ = Convert.ToSingle(jsonData.lordRotationZ);
				lordRotation = new Vector3(rotationX, rotationY, rotationZ);
				
				scriptsInfoDic = jsonData.scriptsInfoDic;
			}
			else
			{
				Debug.LogError("Wrong json data!");
			}
		}
	}
}

