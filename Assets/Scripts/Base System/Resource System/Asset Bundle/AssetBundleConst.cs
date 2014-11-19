using UnityEngine;
using System;

public class AssetBundleConst
{
	public const string TEXTURE_PATH = "/Mapblocks/Textures/";
	public const string MATERIAL_PATH = "/Mapblocks/Materials/";
	public const string LIGHTMAPPING_PATH = "/Lightmappings/";
	public const string MODEL_PATH = "/Mapblocks/Models/";
	public const string PREFAB_PATH = "/Mapblocks/Prefabs/";
	public const string OTHERS_PATH = "/Others/";

	public const string BUNDLE_EXTENSION = ".unity3d";
	public const string META_EXTENSIONG = ".meta";
	public const string LIGHTMAPPING_NAME = "LightmapFar-";

	public const string TEXTURE_MATERIAL_NAME = "TexturesAndMaterialBundle";
	public const string GLOBAL_NAME = "GlobalBundle";
	
	public static string LOAD_PATH = Application.streamingAssetsPath + "/";
}

