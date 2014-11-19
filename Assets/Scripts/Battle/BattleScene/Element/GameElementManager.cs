using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameElementManager : MonoBehaviour
{
	public float preserveDuration = 5f;
	public float tickInterval = 3f;

	//Effect
	public Transform effectParent;
	private Dictionary<int, ElementController> effectPrefabList = new Dictionary<int, ElementController>();

	//FlyingSoldier
	public Transform flyingSoldierParent;
	private Dictionary<int, ElementController> soldierPrefabList = new Dictionary<int, ElementController>();

	//TombStone
	public Transform tombStoneParent;
	public List<ElementController> tombStonePrefabList;

	//BattleLabel
	public Transform battleLabelParent;
	public List<ElementController> battleLabelPrefabList;

	private Dictionary<ElementType, GameElementPool> _elementPoolDic = new Dictionary<ElementType, GameElementPool>();

	private GameObject _tmpPrefabs;

	public void Initialize (List<HeroMeta> heros, List<TroopDefinition> troopDefinitions)
	{
		this._tmpPrefabs = new GameObject("_tmpPrefabs");

		foreach (HeroMeta heroMera in heros) 
		{
			foreach (KeyValuePair<int, int> levelSkillPair in heroMera.skillDic)
			{
				SkillMeta skillMeta = MetaManager.Instance.GetSkillMeta(levelSkillPair.Value);
				RecordMetaEffect(skillMeta.effectID);
			}
		}

		foreach (TroopDefinition troopDefinition in troopDefinitions)
		{
			SoldierMeta soldierMeta = troopDefinition.soldlerMeta;
			BulletMeta bulletMeta = soldierMeta.bulletMeta;

			if (soldierMeta.sizeLevel == SizeLevel.Small && !soldierPrefabList.ContainsKey(soldierMeta.id))
			{
				GameObject soldierPrefab = ResourceFacade.Instance.LoadPrefab(PrefabType.Soldier, soldierMeta.prefabPath);
				ElementController elementController = soldierPrefab.GetComponent<ElementController>();
				elementController.metaID = soldierMeta.id;
				soldierPrefab.transform.parent = _tmpPrefabs.transform;
				soldierPrefab.SetActive(false);

				if (elementController != null)
				{
					soldierPrefabList.Add(soldierMeta.id, elementController);
				}
				else
				{
					Debug.Log("Missing ElementController at " + soldierPrefab.name);
				}
			}

			if (bulletMeta != null)
			{
				foreach (int effectMetaID in new int[] { bulletMeta.effectID, bulletMeta.areaEffectID, bulletMeta.additionEffectID })
				{
					if (effectMetaID > 0)
					{
						RecordMetaEffect(effectMetaID);
					}
				}
			}
		}

		AddElementPool(ElementType.Effect, effectPrefabList, effectParent);

		AddElementPool(ElementType.FlySoldier, soldierPrefabList, flyingSoldierParent);

		AddElementPool(ElementType.BattleLabel, battleLabelPrefabList, battleLabelParent);
		
		AddElementPool(ElementType.TombStone, tombStonePrefabList, tombStoneParent);

		StartCoroutine(UpdateElementPool());
	}

	private void RecordMetaEffect (int effectMetaID)
	{
		EffectMeta effectMeta = MetaManager.Instance.GetEffectMeta (effectMetaID);

		if (!effectPrefabList.ContainsKey(effectMetaID))
		{
			GameObject bulletPrefab = ResourceFacade.Instance.LoadPrefab(PrefabType.Effect, effectMeta.prefabPath);
			ElementController elementController = bulletPrefab.GetComponent<ElementController>();
			elementController.metaID = effectMetaID;
			bulletPrefab.transform.parent = _tmpPrefabs.transform;
			bulletPrefab.SetActive(false);
			
			if (elementController != null)
			{
				effectPrefabList.Add(effectMetaID, elementController);
			}
			else
			{
				Debug.Log("Missing ElementController at " + bulletPrefab.name);
			}
		}
	}

	private void AddElementPool(ElementType elementType, Dictionary<int, ElementController> elementDict, Transform elementParent)
	{
		GameElementPool elementPool = new GameElementPool(elementParent);

		foreach (KeyValuePair<int, ElementController> elementPair in elementDict)
		{
			ElementController elementPrefab = elementPair.Value;
			elementPool.AddElementList(elementPrefab, elementPrefab.cacheNum, elementPrefab.preserveDuration);
		}

		_elementPoolDic[elementType] = elementPool;
	}

	private void AddElementPool(ElementType elementType, List<ElementController> elementList, Transform elementParent)
	{
		ElementController elementPrefab;
		GameElementPool elementPool = new GameElementPool(elementParent);
		
		for (int i = 0; i < elementList.Count; ++i)
		{
			elementPrefab = elementList[i];
			
			if (elementPrefab != null)
			{
				elementPool.AddElementList(elementPrefab, elementPrefab.cacheNum, elementPrefab.preserveDuration);
			}
		}
		_elementPoolDic[elementType] = elementPool;
	}

	private IEnumerator UpdateElementPool ()
	{
		while (true)
		{
			yield return new WaitForSeconds(tickInterval);

			foreach (GameElementPool elementPool in _elementPoolDic.Values)
			{
				elementPool.Tick();
			}
		}
	}

	public ElementController GetElement (ElementType elementType, int metaID)
	{
		GameElementPool elementPool = _elementPoolDic[elementType];
		ElementController elementController =  elementPool.GetElementByID (metaID);

		return elementController;
	}

	public void RecycleElement(ElementController recycler)
	{
		GameElementPool elementPool = _elementPoolDic[recycler.elementType];
		elementPool.RecycleElement(recycler);
	}
}
