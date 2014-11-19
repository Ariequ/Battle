using UnityEngine;
using System;
using System.Collections.Generic;

using LitJson;

public class AssetBundleTest : MonoBehaviour
{
	private const string sceneName = "2-1-1";
	private const string blockName = "area_1";
	private AssetBundleManager loader;
	private string resourcePath;
	private Dictionary<string, string[]> blockModelDic;

	private List<UnityEngine.Object> objs = new List<UnityEngine.Object>();

	void Awake ()
	{
		loader = new AssetBundleManager(this);

		resourcePath = ResourceConst.ASSET_BUNDLE_PATH + "Scenes/" + sceneName;

 		loader.Load(resourcePath + "/Others/GlobalBundle.unity3d", loadGlobalCallback, true);
	}

	void loadGlobalCallback(AssetBundle assetBundle)
	{
		TextAsset mapBlockModelsConfig = assetBundle.Load("MapBlockModelsConfig") as TextAsset;
		blockModelDic = JsonMapper.ToObject<Dictionary<string, string[]>>(mapBlockModelsConfig.text);

		loader.Load(resourcePath + "/Mapblocks/Materials/TexturesAndMaterialBundle.unity3d", loadPicturesCallback, true);
	}

	private int nameCounter;
	private int nameListLength;
	void loadPicturesCallback(AssetBundle assetBundle)
	{
		UnityEngine.Object[] pictures = assetBundle.LoadAll();
		foreach (UnityEngine.Object obj in pictures)
		{
			objs.Add(obj);
			Debug.Log(obj);
		}

		string[] nameList = blockModelDic[blockName];
		nameListLength = nameList.Length;
		nameCounter = 0;
		
		foreach (string modelName in nameList)
		{
			loader.Load(resourcePath + "/Mapblocks/Models/" + modelName + ".unity3d", loadModelCallback, true);
		}
	}

	private void loadModelCallback (AssetBundle assetBundle)
	{
		objs.Add(assetBundle.mainAsset);
		Debug.Log(assetBundle.mainAsset);
		
		nameCounter++;
		if (nameCounter == nameListLength)
			loader.Load(resourcePath + "/Mapblocks/Prefabs/" + blockName + ".unity3d", loadPrefabCallback, true);
			
	}

	private void loadPrefabCallback (AssetBundle assetBundle)
	{
		objs.Add(assetBundle.mainAsset);
		Debug.Log(assetBundle.mainAsset);

		Instantiate(assetBundle.mainAsset);
	}
}

