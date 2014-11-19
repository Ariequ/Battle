using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

using Pathfinding;

using LitJson;

public class LegendFactory
{
	public delegate void LoadCompleteCallback (UnityEngine.Object obj);

	private string SCENE_PATH;
	private string ELEMENT_PATH;
	private Dictionary<string, string[]> blockModelDic;

	private LegendVO legendVO;

	private ResourceManager resourceManager;

	public LegendFactory (MonoBehaviour facade, LegendVO legendVO)
	{
		resourceManager = new ResourceManager(facade);

		this.legendVO = legendVO;

		SCENE_PATH = ResourceConst.ASSET_BUNDLE_PATH + "Scenes/" + SceneNameConst.LEGEND_SCENE_ + "_" + legendVO.id.ToString();
		ELEMENT_PATH = ResourceConst.ASSET_BUNDLE_PATH + "Elements/";

		resourceManager.StartTicking();
	}

	public void Dispose ()
	{
		resourceManager.Dispose();
	}

	#region Configs & Global

	/// <summary>
	/// 载入Global包。这里包括各种Config，寻路信息，Global Prefab
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void LoadGlobal (LoadCompleteCallback callback)
	{
		resourceManager.LoadAssetBundle(SCENE_PATH + "/Others/ConfigsBundle.unity3d", (AssetBundle assetBundle) => {
			UnityEngine.Object[] objList = assetBundle.LoadAll();
			foreach (UnityEngine.Object obj in objList)
			{
				handleConfig(obj as TextAsset);
			}
			loadRoad();
			loadGlobalPrefab(callback);
		});
	}

	private void handleConfig (TextAsset textAsset)
	{
		MapElementType elementType;
		switch(textAsset.name)
		{
		case LegendConst.MAP_BLOCKS_CONFIG: 
			handleMapBlockConfig(textAsset.text);
			break;
		case LegendConst.MAP_BLOCKS_MODEL_CONFIG:
			handleMapBlockModelConfig(textAsset.text);
			break;
		case LegendConst.BUILDINGS_CONFIG:		
			elementType = MapElementType.Building;
			handleMapElementConfig(elementType, textAsset.text);
			break;
		case LegendConst.MONSTERS_CONFIG:		
			elementType = MapElementType.Monster;
			handleMapElementConfig(elementType, textAsset.text);
			break;
		case LegendConst.TREASURES_CONFIG:		
			elementType = MapElementType.Treasure;
			handleMapElementConfig(elementType, textAsset.text);
			break;
		case LegendConst.MAP_GLOBAL_CONFIG:
			handleGlobalConfig(textAsset.text);
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// 处理地块配置信息。
	/// 根据位置和长宽信息，放入裁剪网格，备用。
	/// </summary>
	/// <param name="jsonStr">Json string.</param>
	private void handleMapBlockConfig (string jsonStr)
	{
		MapBlockJsonData[] blockDataArray = JsonMapper.ToObject<MapBlockJsonData[]>(jsonStr);
		List<MapBlockVO> blockVOList = new List<MapBlockVO>();
		for (int i = 0, imax = blockDataArray.Length; i < imax; ++i)
		{
			MapBlockVO blockVO = new MapBlockVO();
			blockVO.JsonData = blockDataArray[i];
			blockVOList.Add(blockVO);

			legendVO.blockCullingTable.AddObject(blockVO, blockVO.Position.x, blockVO.Position.z, blockVO.Extent.x, blockVO.Extent.z);
		}
		legendVO.blockVOList = blockVOList;
	}

	private void handleMapBlockModelConfig (string jsonStr)
	{
		blockModelDic = JsonMapper.ToObject<Dictionary<string, string[]>>(jsonStr);
	}

	/// <summary>
	/// 处理地块元素
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="jsonStr">Json string.</param>
	private void handleMapElementConfig (MapElementType type, string jsonStr)
	{
		MapElementJsonData[] elementDataArray = JsonMapper.ToObject<MapElementJsonData[]>(jsonStr);
		List<MapElementVO> elementVOList = new List<MapElementVO>();
		for (int i = 0, imax = elementDataArray.Length; i < imax; ++i)
		{
			int elementID = Convert.ToInt32(elementDataArray[i].id);

			if (MapElementType.Monster == type && CheckMonsterDefeated(elementID) || 
			    MapElementType.Treasure == type && CheckTreasureDefeated(elementID))
			{
				continue;
			}

			MapElementVO elementVO = new MapElementVO();
			elementVO.JsonData = elementDataArray[i];
			elementVOList.Add(elementVO);

			legendVO.elementCullingTable.AddObject(elementVO, elementVO.Position.x, elementVO.Position.z, 1f, 1f);
		}
		legendVO.elementVOListDic[type] = elementVOList;
	}

	/// <summary>
	/// 处理全局配置
	/// </summary>
	/// <param name="jsonStr">Json string.</param>
	private void handleGlobalConfig (string jsonStr)
	{
		MapGlobalJsonData globalData = JsonMapper.ToObject<MapGlobalJsonData>(jsonStr);
		MapGlobalVO globalVO = new MapGlobalVO();
		globalVO.JsonData = globalData;
		legendVO.globalVO = globalVO;

		//给光照贴图列表设置一个初始长度
		LightmapData[] lightmapList = new LightmapData[globalData.maxLightmapIndex];
		for (int i = 0, imax = lightmapList.Length; i < imax; ++i)
		{
			lightmapList[i] = new LightmapData();
		}
		LightmapSettings.lightmaps = lightmapList;
	}

	/// <summary>
	/// 加载道路的碰撞体
	/// </summary>
	private void loadRoad ()
	{
		resourceManager.LoadAssetBundle(SCENE_PATH + "/Others/RoadsBundle.unity3d", (AssetBundle assetBundle) => {
			UnityEngine.Object[] objList = assetBundle.LoadAll();
			foreach (UnityEngine.Object obj in objList)
			{
				if (obj is GameObject)
					GameObject.Instantiate(obj);
			}
		});
	}

	/// <summary>
	/// 加载Global Prefab
	/// </summary>
	/// <param name="callback">Callback.</param>
	private void loadGlobalPrefab (LoadCompleteCallback callback)
	{
		string path = SCENE_PATH + "/Others/GlobalBundle.unity3d";
		resourceManager.LoadAssetBundle(path, (AssetBundle assetBundle) => {

			UnityEngine.Object globalPrefab = assetBundle.Load("Global");
			GameObject globalObj = GameObject.Instantiate(globalPrefab) as GameObject;

			//为摄像机加上指定的滤镜并设置指定的参数
			Camera mainCamera = globalObj.GetComponentInChildren<Camera>();
			mainCamera.gameObject.AddComponent("JSScriptParser");
			mainCamera.gameObject.AddComponent<CSScriptParser>();
			mainCamera.gameObject.AddComponent<UIRelativeCamera>();
			mainCamera.SendMessage("initParser", legendVO.globalVO.scriptsInfoDic);

			if (callback != null)
				callback.Invoke(globalObj);
		});
	}

	private bool CheckMonsterDefeated(int elementID)
	{
		List<int> defeatedList = legendVO.defeatedMonsterIDList;
		return defeatedList != null && defeatedList.Contains(elementID);
	}
	
	private bool CheckTreasureDefeated(int elementID)
	{
		List<int> defeatedList = legendVO.defeatedTreasureIDList;
		return defeatedList != null && defeatedList.Contains(elementID);
	}

	public byte[] LoadNavMeshData ()
	{
		byte[] bytes;
		string path = SCENE_PATH + "/Others/NavMeshData.zip";
		using (FileStream fs = File.Open(path, FileMode.Open))
		{
			bytes = new byte[fs.Length];
			fs.Read(bytes, 0, bytes.Length);
			fs.Close();
		}
		return bytes;
	}

	#endregion

	#region Mapblock

	/// <summary>
	/// 加载地块。先加载贴图和材质（只加载一次），再加载Prefab，最后加载模型网格和光照贴图。Prefab和模型网格都会由CacheManager缓存
	/// </summary>
	/// <param name="blockID">地块ID.</param>
	/// <param name="callback">Callback.</param>
	public void LoadMapblock (int blockID, LoadCompleteCallback callback)
	{
		loadTextureAndMaterials(blockID, callback);
	}

	private bool materialLoaded = false;
	private void loadTextureAndMaterials (int blockID, LoadCompleteCallback callback)
	{
		if (!materialLoaded)
		{
			resourceManager.LoadAssetBundle(SCENE_PATH + "/Mapblocks/Materials/TextureAndMaterialBundle.unity3d", (AssetBundle assetBundle) => {
				materialLoaded = true;
				UnityEngine.Object[] objList = assetBundle.LoadAll();

				loadMapblockPrefab(blockID, callback);
			}, false);
		}
		else
		{
			loadMapblockPrefab(blockID, callback);
			
		}
	}

	private void loadMapblockPrefab (int blockID, LoadCompleteCallback callback)
	{
		string blockName = "area_" + blockID.ToString();
		string configName = blockName + "_config";
		
		GameObject mapblock = resourceManager.RetrieveCache(ObjectType.GameObject, blockName) as GameObject;
		TextAsset config = resourceManager.RetrieveCache(ObjectType.Config, configName) as TextAsset;
		if (mapblock == null || config == null)
		{
			resourceManager.LoadAssetBundle(SCENE_PATH + "/Mapblocks/Prefabs/" + blockName + ".unity3d", (AssetBundle assetBundle) => {
				mapblock = GameObject.Instantiate(assetBundle.mainAsset) as GameObject;
				mapblock.name = blockName;
				resourceManager.SaveCache(mapblock, 5f);
				mapblock.isStatic = true;
				mapblock.layer = Layers.GROUND;

				config = assetBundle.Load(configName) as TextAsset;
				resourceManager.SaveCache(config, 5f);

				loadMapblockResource(mapblock, config.text, blockID, callback);
			});
		}
		else
		{
			loadMapblockResource(mapblock, config.text, blockID, callback);
		}
	}

	private void loadMapblockResource (GameObject mapblock, string mapblockConfig, int blockID, LoadCompleteCallback callback)
	{
		string blockName = "area_" + blockID.ToString();

		string[] meshNameArray = mapblockConfig.Split('\n');

		Transform mapblockTransform = mapblock.transform;
		int counter = 0;
		for (int i = 0, imax = meshNameArray.Length; i < imax; ++i)
		{
			string meshName = meshNameArray[i];
			if (meshName == "")
				continue;

			GameObject gameObject = i == 0 ? mapblock : mapblockTransform.GetChild(i - 1).gameObject;

			Mesh meshInstance = resourceManager.RetrieveCache(ObjectType.Mesh, meshName) as Mesh;
			if (meshInstance == null)
			{
				counter++;
				resourceManager.LoadAssetBundle(SCENE_PATH + "/Mapblocks/Models/" + meshName + ".unity3d", (AssetBundle assetBundle) => {
					Mesh mesh = assetBundle.mainAsset as Mesh;
					meshInstance = UnityEngine.Object.Instantiate(mesh) as Mesh;
					meshInstance.name = mesh.name;
					
					string uv2Name = mesh.name + "_uv2";
					TextAsset uv2Info = assetBundle.Load(uv2Name) as TextAsset;
					if (uv2Info != null)
					{
						meshInstance.uv2 = getUV2Arrary(uv2Info.text);
					}
					
					resourceManager.SaveCache(meshInstance, 6f);
					gameObject.GetComponent<MeshFilter>().sharedMesh = meshInstance;

					counter--;
					if (counter == 0 && callback != null)
						callback.Invoke(mapblock);
				});
				
			}
			else
			{
				gameObject.GetComponent<MeshFilter>().sharedMesh = meshInstance;
				if (callback != null)
					callback.Invoke(mapblock);
			}
		}

		int[] indexList = legendVO.blockVOList[blockID - 1].IndexList;
		loadLightmapping(indexList);
	}

	private Vector2[] getUV2Arrary(string uv2String)
	{
		string[] uv2StrArray = uv2String.Split('\n');
		Vector2[] uv2Array = new Vector2[uv2StrArray.Length];
		for (int i = 0, imax = uv2StrArray.Length; i < imax; ++i)
		{
			if (uv2StrArray[i].Length > 0)
			{
				string[] uv2StrValue = uv2StrArray[i].Split(',');
				Vector2 uv2 = new Vector2();
				uv2.x = Convert.ToSingle(uv2StrValue[0]);
				uv2.y = Convert.ToSingle(uv2StrValue[1]);
				uv2Array[i] = uv2;
			}
		}
		return uv2Array;
	}

	public void UnloadMapblock (int blockID, GameObject mapblock)
	{
		Transform mapblockTransform = mapblock.transform;
		int childCount = mapblockTransform.childCount;
		for (int i = -1; i < childCount; ++i)
		{
			Transform child = i == -1 ? mapblockTransform : mapblockTransform.GetChild(i);
			Mesh mesh = child.GetComponent<MeshFilter>().sharedMesh;
			resourceManager.ReleaseCache(mesh);
		}
		resourceManager.ReleaseCache(mapblock);

		int[] indexList = legendVO.blockVOList[blockID - 1].IndexList;
		unloadLightmapping(indexList);
	}

	#endregion

	#region Map Elements

	/// <summary>
	/// 加载地图元素（建筑，怪，宝箱）
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="elementID">Element I.</param>
	/// <param name="callback">Callback.</param>
	public void LoadMapElement (MapElementType type, int elementID, LoadCompleteCallback callback)
	{
		string typeName = type.ToString();
		string elementName = typeName + "_" + elementID.ToString();

		MapElementVO elementVO = legendVO.elementVOListDic[type][elementID - 1];
		loadLightmapping(new int[]{elementVO.LightmapIndex});

		GameObject mapElement = resourceManager.RetrieveCache(ObjectType.GameObject, elementName) as GameObject;
		if (mapElement == null)
		{
			string path = ELEMENT_PATH + typeName + "s/" + elementName + ".unity3d";
			resourceManager.LoadAssetBundle(path, (AssetBundle assetBundle) => {

				//TODO: Load uv2

				mapElement = GameObject.Instantiate(assetBundle.mainAsset) as GameObject;
				mapElement.name = elementName;
				resourceManager.SaveCache(mapElement, 5f);

				handleMapElement(elementVO, mapElement);

				if (callback != null)
					callback.Invoke(mapElement);
			});
		}
		else
		{
			if (callback != null)
				callback.Invoke(mapElement);
		}
	}

	public void UnloadMapElement (MapElementType type, int elementID, UnityEngine.Object mapElement)
	{
		resourceManager.ReleaseCache(mapElement);

		MapElementVO elementVO = legendVO.elementVOListDic[type][elementID - 1];
		unloadLightmapping(new int[]{elementVO.LightmapIndex});
	}

	private void handleMapElement(MapElementVO elementVO, GameObject mapElement)
	{
		mapElement.transform.position = elementVO.Position;
		mapElement.AddComponent<MapElementView>();
		mapElement.isStatic = true;

		Renderer render = mapElement.GetComponentInChildren<Renderer>();
		render.lightmapIndex = elementVO.LightmapIndex;
		render.lightmapTilingOffset = elementVO.LightmapTilingOffset;

		switch(elementVO.ElementType)
		{
		case MapElementType.Building:
			break;
		case MapElementType.Monster:
			mapElement.layer = Layers.ARMY;
			//NavmeshCut就是寻路的障碍物。
			NavmeshCut cut = mapElement.AddComponent<NavmeshCut>();
			cut.height = 10;
			cut.type = NavmeshCut.MeshType.Circle;
			cut.circleRadius = 2.5f;
			cut.circleResolution = 6;
			break;
		case MapElementType.Treasure:
			break;
		}

	}

	#endregion

	#region Lightmapping

	private void loadLightmapping(int[] indexList)
	{
		for (int i = 0, imax = indexList.Length; i < imax; ++i)
		{
			int lightmapIndex = indexList[i];
			if (lightmapIndex >= 0)
			{
				string lightmapName = LegendConst.LIGHTMAP_NAME + lightmapIndex.ToString();
				Texture2D lightmap = resourceManager.RetrieveCache(ObjectType.Texture, lightmapName) as Texture2D;

				if (lightmap == null)
				{
					string path = SCENE_PATH + "/Lightmappings/" + lightmapName + ".unity3d";
					resourceManager.LoadAssetBundle(path, (AssetBundle assetBundle) => {
						lightmap = UnityEngine.Object.Instantiate(assetBundle.mainAsset) as Texture2D;
						lightmap.name = lightmapName;
						resourceManager.SaveCache(lightmap, 6f);

						int index = CommonUtil.GetNumFromStringEnd(lightmap.name, '-');
						setLightmap(lightmap, index);
					});
				}
				else
				{
					setLightmap(lightmap, lightmapIndex);
				}
			}
		}
	}

	private void setLightmap(Texture2D lightmap, int index)
	{
		LightmapData[] lightmaps = LightmapSettings.lightmaps;
		if (index >= 0 && index <= lightmaps.Length - 1)
		{
			lightmaps[index].lightmapFar = lightmap;
			LightmapSettings.lightmaps = lightmaps;
		}
	}

	private void unloadLightmapping(int[] indexList)
	{
		LightmapData[] lightmapDataList = LightmapSettings.lightmaps;
		
		for (int i = 0, imax = indexList.Length; i < imax; ++i)
		{
			int index = indexList[i];
			if (index >= 0)
			{
				LightmapData lightmapData = lightmapDataList[index];
				resourceManager.ReleaseCache(lightmapData.lightmapFar, () => {
					LightmapData[] dataList = LightmapSettings.lightmaps;
					dataList[index].lightmapFar = null;
					LightmapSettings.lightmaps = dataList;
				});
			}
		}

	}

	#endregion
}

