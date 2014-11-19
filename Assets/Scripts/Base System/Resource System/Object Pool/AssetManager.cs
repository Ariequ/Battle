using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AssetManager主要用于管理数量众多的一样的游戏对象。例如士兵，子弹等。根据需求数量会动态地调整。
/// </summary>
public class AssetManager
{
	public float tickInterval = 1f;

	private Dictionary<ObjectType, AssetPool> assetPoolDic;

	public AssetManager ()
	{
		assetPoolDic = new Dictionary<ObjectType, AssetPool>();
	}

	public void Dispose ()
	{
		foreach (AssetPool pool in assetPoolDic.Values)
		{
			pool.Dispose();
		}

		assetPoolDic.Clear();
	}

	/// <summary>
	/// 预先对游戏对象进行缓存。
	/// </summary>
	/// <param name="type">对象类型</param>
	/// <param name="asset">Asset.</param>
	/// <param name="life">回收周期</param>
	/// <param name="maxPreloadCount">缓存数量</param>
	/// <param name="growth">增长速度</param>
	public void PreloadAsset (ObjectType type, GameObject asset, float life, int maxPreloadCount, int growth)
	{
		AssetPool pool = null;
		assetPoolDic.TryGetValue(type, out pool);
		
		if (pool == null)
		{
			pool = new AssetPool();
		}
		pool.Preload(asset, type, life, maxPreloadCount, growth);
		assetPoolDic[type] = pool;
	}

	/// <summary>
	/// 加载一个游戏对象.
	/// </summary>
	/// <returns>The asset.</returns>
	/// <param name="type">对象类型</param>
	/// <param name="assetName">Asset name.</param>
	public GameObject LoadAsset (ObjectType type, string assetName)
	{
		AssetPool pool = null;
		assetPoolDic.TryGetValue(type, out pool);

		if (pool != null)
		{
			GameObject asset = pool.Shift(assetName);
			return asset;
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	/// 回收一个游戏对象
	/// </summary>
	/// <param name="asset">Asset.</param>
	public void RecycleAsset (GameObject asset)
	{
		AssetInfo assetInfo = asset.GetComponent<AssetInfo>();

		if (assetInfo != null)
		{
			AssetPool pool = null;
			assetPoolDic.TryGetValue(assetInfo.type, out pool);

			if (pool != null)
			{
				pool.Add(assetInfo);
			}
			else
			{
				Debug.LogError("Asset can't find the its pool: " + asset.ToString() + " of type: " + assetInfo.type);
			}
		}
		else
		{
			Debug.LogError("Wrong asset to recycle: " + asset.ToString());
		}
	}

	public IEnumerator Tick()
	{
		while (true)
		{
			yield return new WaitForSeconds(tickInterval);

			foreach (AssetPool pool in assetPoolDic.Values)
			{
				pool.Tick(tickInterval);
			}
		}
	}
}

