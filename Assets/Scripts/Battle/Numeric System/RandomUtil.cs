using System;
using UnityEngine;

public class RandomUtil
{
	public const float B_FLOAT = 10000f;
	public const int B_INT = 10000;

	private static System.Random systemRandom = new System.Random ();

	public static int Range(int min, int max)
	{
		return systemRandom.Next (min, max);
	}

	public static float Range(float min, float max)
	{
		return value * (max - min) + min;

//		return UnityEngine.Random.Range(min, max);
	}

	public static float value
	{
		get
		{
//			return UnityEngine.Random.value;
			return systemRandom.Next (0, B_INT) / B_FLOAT;
		}
	}

    public static Vector3 onUnitSphere
    {
        get
        {
            return UnityEngine.Random.onUnitSphere;
        }
    }
}

