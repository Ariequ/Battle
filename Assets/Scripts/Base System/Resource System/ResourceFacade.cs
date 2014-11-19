using UnityEngine;

using System;
using System.Collections.Generic;

public enum PrefabType
{
    MapBlock,
    MapElement,
    Effect,
    Soldier,
    Monster,
    BattleLabel,
    Tombstone,
    WorldMap,
    UI
}

/// <summary>
/// 这个类其实并没有其实质的作用。应该用Object Pool里面的类来替换。
/// </summary>
public class ResourceFacade 
{
	public const string PREFAB_PATH = "Prefabs/";

    private static ResourceFacade instance;

	private Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

    public static ResourceFacade Instance
    {
        get
        {
            if (instance == null)
                instance = new ResourceFacade();
            return instance;
        }
    }

	public GameObject LoadPrefab(string localPath)
	{
		GameObject prefab = null;
		prefabDict.TryGetValue (localPath, out prefab);

		if (prefab == null)
		{
			prefab = Resources.Load<GameObject>(PREFAB_PATH + localPath);
			prefabDict.Add(localPath, prefab);
		}

		GameObject gameObject = GameObject.Instantiate(prefab) as GameObject;
		
		return gameObject;
	}

	public GameObject LoadPrefab(PrefabType prefabType, string localPath)
    {
		return LoadPrefab(localPath);
    }

	public void Unload(GameObject gameObject)
	{
		GameObject.Destroy(gameObject);
	}
	
	public void Unload(GameObject gameObject, PrefabType prefabType, string name)
    {
        GameObject.Destroy(gameObject);
    }

	public LightProbes LoadLightProbes(string localPath)
	{
		return Resources.Load<LightProbes>(PREFAB_PATH + localPath);
	}

    public Texture2D LoadTexture2D(string localPath)
    {
        Texture2D texture2D = Resources.Load<Texture2D>(localPath);
        return texture2D;
    }

    public void Unload(Texture2D texture2D)
    {
		Debug.Log ("Unload Texture2D " + texture2D.name);
    }

    public TextAsset LoadTextAsset(string localPath)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(localPath);
        return textAsset;
    }

    public void Unload(TextAsset textAsset)
    {
        UnityEngine.Object.Destroy(textAsset);
    }
}
