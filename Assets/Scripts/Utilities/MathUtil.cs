using System;
using UnityEngine;

/// <summary>
/// All useful math utilities...
/// </summary>
public class MathUtil
{
	public static Vector3 GetXZPosition (Vector3 position)
	{
		return new Vector3(position.x, 0f, position.z);
	}

	public static Vector3 GetTerrainPosition (Vector3 position)
	{
#if BATTLE_SIMULATOR
        return new Vector3(position.x, 0, position.z);
#endif

		TerrainData terrainData =  Terrain.activeTerrain.terrainData;
		float height = terrainData.GetInterpolatedHeight(position.x, position.z);
		return new Vector3(position.x, height, position.z);
	}

	public static float DistanceXZ (Vector3 from, Vector3 to)
	{
		float x = from.x - to.x;
		float z = from.z - to.z;

		return Mathf.Sqrt(x * x + z * z);
	}

	public static float SqrDistanceXZ (Vector3 from, Vector3 to)
	{
		float x = from.x - to.x;
		float z = from.z - to.z;

		return x * x + z * z;
	}

	public static float SqrDistanceUI (Vector3 from, Vector3 to)
	{
		float x = from.x - to.x;
		float y = from.y - to.y;

		return x * x + y * y;
	}

	public static Quaternion LookRotationXZ (Vector3 direction)
	{
		return Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
	}

    public static Battle.Vector2 ParseToVector2(Vector3 vec3)
    {
        return new Battle.Vector2(vec3.x, vec3.z);
    }

    public static Vector3 ParseToVector3(Battle.Vector2 vec2, float height = 0)
    {
		return new Vector3(vec2.x(), height, vec2.y());
    }
}

