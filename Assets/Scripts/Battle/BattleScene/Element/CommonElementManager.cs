using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A simpler element manager. Use it to cache some game object quickly.
/// </summary>
public class CommonElementManager : MonoBehaviour
{
	public float preserveDuration = 5f;
	public float tickInterval = 3f;

	public string elementPath;

	public Transform elementParent;
	public List<ElementController> elementPrefabList;

	private GameElementPool _elementPool;

	void Awake ()
	{
		_elementPool = new GameElementPool(elementParent);

		ElementController elementPrefab;

		if (elementPath != null)
		{
			ElementController[] allElements = Resources.LoadAll<ElementController>(elementPath);

			for (int i = 0; i < allElements.Length; ++i)
			{
				elementPrefab = allElements[i];

				if (elementPrefab != null)
				{
					_elementPool.AddElementList(elementPrefab, elementPrefab.cacheNum, elementPrefab.preserveDuration);
				}
			}
		}

		for (int i = 0; i < elementPrefabList.Count; ++i)
		{
			elementPrefab = elementPrefabList[i];
			
			if (elementPrefab != null)
			{
				_elementPool.AddElementList(elementPrefab, elementPrefab.cacheNum, elementPrefab.preserveDuration);
			}
		}


		//Start update element pool. Cut down the pool's size timely.
		StartCoroutine(UpdateElementPool());
	}

	
	private IEnumerator UpdateElementPool ()
	{
		while (true)
		{
			yield return new WaitForSeconds(tickInterval);
			
			_elementPool.Tick();
		}
	}
	
	public ElementController GetElement (int metaID)
	{
		return _elementPool.GetElementByID(metaID);
	}
	
	public void RecycleElement(ElementController recycler)
	{
		_elementPool.RecycleElement(recycler);
	}

	public void RecycleAll ()
	{
		if (elementParent != null)
		{
			ElementController[] elementControllers = elementParent.GetComponentsInChildren<ElementController>();
			
			foreach (ElementController elementController in elementControllers)
			{
				RecycleElement(elementController);
			}
		}
	}
}

