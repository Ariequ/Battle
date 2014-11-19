using UnityEngine;
using System;
using System.Collections.Generic;

public class GameElementPool
{
	private Dictionary<int, GameElementList> _elementListDic;

	public GameElementPool (Transform parent)
	{
		this.parentTransform = parent;

		_elementListDic = new Dictionary<int, GameElementList>();
	}

	public Transform parentTransform { get; private set; }

	public void AddElementList(ElementController elementPrefab, int cacheNum, float preserveDuration)
	{
		if (! _elementListDic.ContainsKey(elementPrefab.metaID))
		{
			_elementListDic[elementPrefab.metaID] = new GameElementList(elementPrefab, parentTransform, cacheNum, preserveDuration);
		}
	}

	public ElementController GetElementByID(int metaID)
	{
		GameElementList elementList = null;
		_elementListDic.TryGetValue(metaID, out elementList);

		return elementList != null ? elementList.Pop() : null;
	}

	public void RecycleElement(ElementController element)
	{
		GameElementList elementList = null;
		_elementListDic.TryGetValue(element.metaID, out elementList);

		if (elementList != null)
		{
			elementList.Push(element);
		}
	}

	public void Tick()
	{
		foreach (GameElementList elementList in _elementListDic.Values)
		{
			elementList.Tick();
		}
	}
}
