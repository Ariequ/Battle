using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// AssetBundle的加载管理器。每一个AssetBundle只有唯一一个Loader去加载。防止重复加载。
/// </summary>
public class AssetBundleManager
{
	public delegate void AssetBundleCompleteCallback (AssetBundle assetBundle);
	
	private Dictionary<string, AssetBundleLoader> loaderDic;

	private MonoBehaviour facade;

	public AssetBundleManager(MonoBehaviour facade)
	{
		this.facade = facade;
		
		loaderDic = new Dictionary<string, AssetBundleLoader>();
	}

	public void Dispose ()
	{
		foreach (AssetBundleLoader loader in loaderDic.Values)
		{
			loader.Dispose();
		}
		loaderDic.Clear();
	}

	public void Load(string path, AssetBundleCompleteCallback callback, bool autoDestroy)
	{
		AssetBundleLoader loader;
		if (!loaderDic.ContainsKey(path))
		{
			loader = new AssetBundleLoader(path, facade);
			loader.LoadAssetBundle(callback, autoDestroy);
			loaderDic[path] = loader;
		}
		else
		{
			loader = loaderDic[path];
			if (loader.isLoading)
			{
				loader.AddCallback(callback);
			}
			else
			{
				loader.LoadAssetBundle(callback, autoDestroy);
			}
		}
		
	}
}

