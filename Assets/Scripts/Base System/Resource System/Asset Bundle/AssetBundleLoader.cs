
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Thrift.Collections;

/// <summary>
/// AssetBundle加载器。保存一个回调列表。加载完成后依次触发回调。
/// </summary>
public class AssetBundleLoader
{


	public bool isLoading;

	private string path;

	private AssetBundle assetBundle;

	private List<AssetBundleManager.AssetBundleCompleteCallback> callbackList;

	private MonoBehaviour facade;

	public AssetBundleLoader (string path, MonoBehaviour facade)
	{
		this.path = path;
		this.facade = facade;

		callbackList = new List<AssetBundleManager.AssetBundleCompleteCallback>();
	}

	public void Dispose ()
	{
		if (assetBundle != null)
			assetBundle.Unload(false);

		callbackList.Clear();
	}

	public void LoadAssetBundle(AssetBundleManager.AssetBundleCompleteCallback callback, bool autoDestroy = true)
	{
		facade.StartCoroutine(loadAssetBundleCoroutine(path, callback, autoDestroy));
	}
	
	private IEnumerator loadAssetBundleCoroutine(string path, AssetBundleManager.AssetBundleCompleteCallback completeCallback, bool autoDestroy)
	{
		callbackList.Add(completeCallback);
		
		if (assetBundle == null)
		{
			isLoading = true;

			if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
			{
				path = "file://" + path;
			}
			
//			AssetBundleCreateRequest createRequest;
//			using (FileStream fs = File.Open(path, FileMode.Open))
//			{
//				long length = fs.Length;
//				byte[] bytes = new byte[length];
//				fs.Read(bytes, 0, bytes.Length);
//				createRequest = AssetBundle.CreateFromMemory(bytes);
//				fs.Close();
//			}


			Debug.Log("loadAssetBundleCoroutine path : " + path);

			WWW www = new WWW(path);

			yield return www;
			
			assetBundle = www.assetBundle;
			ExecuteCallbackList();

			//让回调飞一会儿。。。
			yield return new WaitForSeconds(2f);
			
			isLoading = false;
			if (autoDestroy)
			{
				www.Dispose();
				assetBundle.Unload(false);
				assetBundle = null;
			}
		}
		else
		{
			ExecuteCallbackList();
		}


	}

	public void AddCallback(AssetBundleManager.AssetBundleCompleteCallback callback)
	{
		callbackList.Add(callback);
	}

	private void ExecuteCallbackList ()
	{
		if (callbackList.Count > 0)
		{
			for (int i = callbackList.Count - 1; i >= 0; --i)
			{
				AssetBundleManager.AssetBundleCompleteCallback callback = callbackList[i];
				callbackList.RemoveAt(i);
				if (callback != null)
					callback.Invoke(assetBundle);
			}
		}
	}
}

