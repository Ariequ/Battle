using UnityEngine;
using System;
using System.Collections.Generic;

public class GameElementList
{
	private ElementController _prefab;

	private Transform _parentTransform;

	private List<ElementController> _elementList;

	private int _cacheSize;

	private int _cacheNum;

	private float _preserveDuration;

	private float _timeStamp;

	public GameElementList (ElementController prefab, Transform parentTransform, int cacheNum = 10, float preserveDuration = 5f)
	{
		_elementList = new List<ElementController>();

		_prefab = prefab;
		_parentTransform = parentTransform;
		_cacheNum = cacheNum;
		_preserveDuration = preserveDuration;
		_timeStamp = BattleTime.time;

		CacheElement(cacheNum);
	}

	public int Count
	{
		get
		{
			return _elementList.Count;
		}
	}

	public ElementController Pop ()
	{
		ElementController element;

		if (_elementList.Count == 0)
		{
			CacheElement();
		}

		element = _elementList[_elementList.Count - 1];
		_elementList.RemoveAt(_elementList.Count - 1);
		element.gameObject.SetActive(true);

		_timeStamp = BattleTime.time;

		return element;
	}

	public void Push (ElementController element)
	{
		element.gameObject.SetActive(false);
		_elementList.Add(element);

		_timeStamp = BattleTime.time;
	}

	public void Tick()
	{
		if (BattleTime.time - _timeStamp > _preserveDuration)
		{
			CutDownElement();
		}
	}

	private void CacheElement (int cacheNum = 1)
	{
		ElementController element;

		for (int i = 0; i < cacheNum; ++i)
		{
			element = GameObject.Instantiate(_prefab) as ElementController;
			element.gameObject.SetActive(false);
			element.metaID = _prefab.metaID;
			element.transform.parent = _parentTransform;
			_elementList.Add(element);
		}

		_cacheSize += cacheNum;
	}

	private void CutDownElement ()
	{
		//Ensure that  cacheSize is more than one cacheNum and all fx has recycled to the list.
		if (_cacheSize > _cacheNum && _elementList.Count == _cacheSize)
		{
			_cacheSize -= _cacheNum;

			int startIndex = _elementList.Count - _cacheNum;

			ElementController elementController;

			lock (this)
			{
				for (int i = startIndex; i < startIndex +_cacheNum; ++i)
				{
					elementController = _elementList[i];
					elementController.transform.parent = null;
					GameObject.Destroy(elementController.gameObject);
				}
				_elementList.RemoveRange(startIndex, _cacheNum);
			}
		}
	}
}
