using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

using LitJson;

public class LegendManager : MonoBehaviour 
{
    public Transform blockContainer;
    public Transform buildingContainer;
    public Transform monsterContainer;
    public Transform treasureContainer;

    private Transform heroTransform;
    private Vector2 heroPosition;

	private LegendVO legendVO;
	private LegendFactory factory;

	private GameObject lordController;
	private GameObject lordHero;

    private CullingTable blockCullingTable;
    private CullingTable elementCullingTable;

    private List<MapBlockVO> prevMapBlockList;
    private List<MapElementVO> prevMapElementList;

    void Awake () 
    {
		heroPosition = new Vector2(-1, -1);

		legendVO = MockServer.worldVO;
		blockCullingTable = legendVO.blockCullingTable;
		elementCullingTable = legendVO.elementCullingTable;

        prevMapBlockList = new List<MapBlockVO>();
        prevMapElementList = new List<MapElementVO>();

		factory = new LegendFactory(this, MockServer.worldVO);

        initializeGlobal();
	}

	/// <summary>
	/// 这里不析构，可是要内存泄露的哦~
	/// </summary>
	void OnDestroy ()
	{
		factory.Dispose();

		blockCullingTable.Dispose();
		elementCullingTable.Dispose();

		prevMapBlockList.Clear();
		prevMapElementList.Clear();
	}

    private void initializeGlobal()
    {
		factory.LoadGlobal((UnityEngine.Object obj) => {

			initializeNavMesh();
			initializeHero();

			UIRelativeCamera relativeCamera = Camera.main.gameObject.AddComponent<UIRelativeCamera>();
			
			relativeCamera.relativeObjects = new GameObject[]
			{
				buildingContainer.gameObject,
				monsterContainer.gameObject,
				treasureContainer.gameObject,
				lordController,
				lordHero
			};

			StartCoroutine(cullMap());
			
			ApplicationFacade.Instance.SendNotification(NotificationConst.WORLDMAP_READY);
		});
    }

	private void initializeHero()
	{
		lordController = ResourceFacade.Instance.LoadPrefab(PrefabType.WorldMap, "WorldMap/ExploreController");
		lordHero = ResourceFacade.Instance.LoadPrefab(PrefabType.WorldMap, "WorldMap/Lord");
		
		LegendCharactorController charactorController = lordController.GetComponent<LegendCharactorController>();
		charactorController.name = LegendCharactorController.NAME;
		charactorController.charactor = lordHero.transform;
		
		heroTransform = lordHero.transform;
		
		CustomSmoothFollow customSmoothFollow = Camera.main.GetComponent<CustomSmoothFollow>();
		customSmoothFollow.target = lordHero.transform;
		
		heroTransform.position = legendVO.globalVO.lordPosition;
		heroTransform.localEulerAngles = legendVO.globalVO.lordRotation;
	}

	private void initializeNavMesh ()
	{
		byte[] navmeshData = factory.LoadNavMeshData();
		AstarPath.active.astarData.DeserializeGraphs(navmeshData);

		GameObject.Find("A*").AddComponent<TileHandlerHelper>();
	}

    private IEnumerator cullMap()
    {
        while (true)
        {
            Vector2 heroNewPosition = blockCullingTable.GetPosition(heroTransform.position);
            if (heroNewPosition != heroPosition)
            {
                heroPosition = heroNewPosition;

				cullMapBlocks(heroNewPosition);
				cullMapElements(heroNewPosition);
            }

            yield return new WaitForSeconds(LegendConst.CULLING_INTERVAL);
        }
	}

	private void cullMapBlocks(Vector2 heroPosition)
    {
		List<MapBlockVO> aroundMapBlockList = getMapBlockVOsAround(heroPosition, LegendConst.SEARCH_RANGE);
        
        foreach (MapBlockVO prevBlockVO in prevMapBlockList)
        {
            if (!aroundMapBlockList.Contains(prevBlockVO))
            {
				//防止地块还没载入完就卸载
				if (prevBlockVO.gameObject != null)
				{
					factory.UnloadMapblock(prevBlockVO.ID, prevBlockVO.gameObject);
	                prevBlockVO.gameObject = null;
				}
            }
        }

		int aroundBlockCount = 0;

        foreach (MapBlockVO aroundBlockVO in aroundMapBlockList)
        {
            if (!prevMapBlockList.Contains(aroundBlockVO))
            {
				//防止重复加载
				if (aroundBlockVO.gameObject == null)
				{
					//防止闭包调用时出错
					MapBlockVO blockVO = aroundBlockVO;
					aroundBlockCount++;
					factory.LoadMapblock(blockVO.ID, (UnityEngine.Object obj) => {
						GameObject mapBlock  = obj as GameObject;
						blockVO.gameObject = mapBlock;
						mapBlock.layer = Layers.EXPLORE_ELEMENTS;
						mapBlock.transform.parent = blockContainer;
						mapBlock.isStatic = true;

						aroundBlockCount--;
						if (aroundBlockCount == 0)
							cullMapBlockComplete();
					});
				}
            }
        }
        
        prevMapBlockList = aroundMapBlockList;
    }

	private void cullMapBlockComplete ()
	{
		//Static Batch不会自动生效，需要手动触发
		StaticBatchingUtility.Combine(blockContainer.gameObject);

		//定期释放不用的资源
		Resources.UnloadUnusedAssets();
	}
    
	private void cullMapElements(Vector2 heroPosition)
    {
		List<MapElementVO> aroundMapElementList = getMapElementVOsAround(heroPosition, LegendConst.SEARCH_RANGE);
        
        foreach (MapElementVO prevElementVO in prevMapElementList)
        {
			if (!aroundMapElementList.Contains(prevElementVO))
			{
				if (prevElementVO.gameObject != null)
	            {
					factory.UnloadMapElement(prevElementVO.ElementType, prevElementVO.ID, prevElementVO.gameObject);
	                prevElementVO.gameObject = null;
	            }
			}
        }

		int aroundElementCount = 0;

        foreach (MapElementVO aroundElementVO in aroundMapElementList)
        {
			if (!prevMapElementList.Contains(aroundElementVO))
			{
	            if (aroundElementVO.gameObject == null)
	            {
					MapElementVO elementVO = aroundElementVO;
//					Debug.Log(elementVO.ID + ": " + elementVO.Position.ToString());
					aroundElementCount++;
					factory.LoadMapElement(aroundElementVO.meta.type, aroundElementVO.ID, (UnityEngine.Object obj) => {
						GameObject mapElement = obj as GameObject;
						elementVO.gameObject = mapElement;
						mapElement.GetComponent<MapElementView>().mapElementVO = elementVO;
						
						Transform container = null;
						switch (elementVO.meta.type)
						{
						case MapElementType.Building:
							container = buildingContainer;
							break;
						case MapElementType.Monster:
							container = monsterContainer;
							break;
						case MapElementType.Treasure:
							container = treasureContainer;
							break;
						}
						
						mapElement.transform.parent = container;

						aroundElementCount--;
						if (aroundElementCount == 0)
							cullMapElementComplete();
					});
	            }
			}
        }

        prevMapElementList = aroundMapElementList;
    }

	private void cullMapElementComplete ()
	{
		//Static Batch不会自动生效，需要手动触发
		StaticBatchingUtility.Combine(buildingContainer.gameObject);
		StaticBatchingUtility.Combine(monsterContainer.gameObject);
		StaticBatchingUtility.Combine(treasureContainer.gameObject);
		//定期释放不用的资源
		Resources.UnloadUnusedAssets();
	}
    
    private List<MapBlockVO> getMapBlockVOsAround(Vector2 position, int range)
    {
        List<Vector2> aroundPositions = blockCullingTable.GetPositionsAround(position, range);

        List<MapBlockVO> resultList = new List<MapBlockVO>();

		if (aroundPositions != null)
		{
	        foreach (Vector2 pos in aroundPositions)
	        {
	            List<object> dataList = blockCullingTable.GetDataList(pos);
	            if (dataList != null)
	            {
	                foreach (object obj in dataList)
	                {
	                    MapBlockVO mapBlockVO = obj as MapBlockVO;
	                    if (!resultList.Contains(mapBlockVO))
	                    {
	                        resultList.Add(mapBlockVO);
	                    }
	                }
	            }
	        }
		}

        return resultList;
    }

    private List<MapElementVO> getMapElementVOsAround(Vector2 position, int range)
    {
        List<Vector2> aroundPositions = elementCullingTable.GetPositionsAround(position, range);
        List<MapElementVO> resultList = new List<MapElementVO>();
        
		if (aroundPositions != null)
		{
	        foreach (Vector2 pos in aroundPositions)
	        {
	            List<object> dataList = elementCullingTable.GetDataList(pos);

	            if (dataList != null)
	            {
					foreach (MapElementVO mapElementVO in dataList)
	                {
						if (!resultList.Contains(mapElementVO))
						{
							resultList.Add(mapElementVO);
						}
					}
				}
			}
		}
		
		return resultList;
	}

}
