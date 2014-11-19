using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager
{
//	private AssetBundleManager assetBundleManager;

	private AssetManager assetManager;

	private CacheManager cacheManager;

	private MonoBehaviour facade;

	public ResourceManager (MonoBehaviour facade)
	{
		this.facade = facade;

//		assetBundleManager = new AssetBundleManager(facade);
		assetManager = new AssetManager();
		cacheManager = new CacheManager();
	}

	public void Dispose ()
	{
//		assetBundleManager.Dispose();
		assetManager.Dispose();
		cacheManager.Dispose();

		facade.StopAllCoroutines();
	}

	public void StartTicking ()
	{
		facade.StartCoroutine(assetManager.Tick());
		facade.StartCoroutine(cacheManager.Tick());
	}

	/// <summary>
	/// 载入一个AssetBundle
	/// </summary>
	/// <param name="path">路径</param>
	/// <param name="objectCallback">对获得的Asset的回调</param>
//	public void LoadAssetBundle(string path, AssetBundleManager.AssetBundleCompleteCallback callback, bool autoDestroy = true)
//	{
//		assetBundleManager.Load(path, callback, autoDestroy);
//	}

	/// <summary>
	/// 对指定游戏对象进行预加载
	/// </summary>
	/// <param name="type">类型.</param>
	/// <param name="asset">指定的对象.</param>
	/// <param name="life">生存时间.</param>
	/// <param name="maxPreloadCount">最大预加载数量.</param>
	/// <param name="growth">对象数量不足时，一次的增长量.</param>
	public void PreloadAsset (ObjectType type, GameObject asset, float life = -1f, int maxPreloadCount = 1, int growth = 1)
	{
		assetManager.PreloadAsset(type, asset, life, maxPreloadCount, growth);
	}

	/// <summary>
	/// 载入游戏对象
	/// </summary>
	/// <param name="type">游戏对象类型.</param>
	/// <param name="assetName">游戏对象名.</param>
	public GameObject LoadAsset (ObjectType type, string assetName)
	{
		return assetManager.LoadAsset(type, assetName);
	}

	/// <summary>
	/// 回收游戏对象
	/// </summary>
	/// <param name="asset">游戏对象.</param>
	public void RecycleAsset (GameObject asset)
	{
		assetManager.RecycleAsset(asset);
	}

	/// <summary>
	/// 预加载缓存对象
	/// </summary>
	/// <param name="cache">缓存对象.</param>
	/// <param name="life">缓存时间.</param>
	public void SaveCache (UnityEngine.Object cache, float remainLife = -1f)
	{
		cacheManager.SaveCache(cache, remainLife);
	}

	/// <summary>
	/// 检索缓存对象
	/// </summary>
	/// <param name="type">缓存类型.</param>
	/// <param name="cacheName">缓存名.</param>
	public UnityEngine.Object RetrieveCache (ObjectType type, string cacheName)
	{
		return cacheManager.RetrieveCache(type, cacheName);
	}

	/// <summary>
	/// 释放缓存
	/// </summary>
	/// <param name="cache">缓存对象.</param>
	/// <param name="destroyCallback">销毁对象时的回调.</param>
	public void ReleaseCache (UnityEngine.Object cache, CacheObject.DestroyDelegate destroyCallback = null)
	{
		cacheManager.ReleaseCache(cache, destroyCallback);
	}
}

