using UnityEngine;
using System;

public enum ObjectType
{
	GameObject,
	Mesh,
	Texture,
	Materal,
	Config,
	None
}

public class ResourceConst
{
	//不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
	public static readonly string PathURL =
		#if UNITY_ANDROID
		"jar:file://" + Application.dataPath + "!/assets/";
	#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	Application.dataPath + "/StreamingAssets/";
	#elif UNITY_IPHONE
	Application.dataPath + "/Raw/";
	#else
	string.Empty;
	#endif

	public static string ASSET_BUNDLE_PATH = PathURL + "Asset Bundles/";
}

