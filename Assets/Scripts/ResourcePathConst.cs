public class ResourcePathConst 
{
	private const string ID_REPLACEMENT = "{id}";

	public const string PREFABS_PATH = "/Resources/Prefabs/";

	public const string BATTLEFIELD_PATH = "Battlefields/" + ID_REPLACEMENT + "/Components/";
	public const string MAPBLOCK_PATH = "Mapblocks/";
	public const string MAPELEMENT_PATH = "MapElements/";

	public const string MAPBLOCK_CONFIG_NAME = "MapBlocksConfig";
	public const string MAPBLOCK_MODEL_CONFIG_NAME = "MapBlockModelsConfig";
	public const string MAPGLOBAL_CONFIG_NAME = "MapGlobalConfig";
	public const string MAPELEMENT_CONFIG_POSTNAME = "sConfig";

	public const string LIGHTMAP_NAME = "LightmapFar-";
	public const string LIGHTPROBES_NAME = "LightProbes";

	public static string GetBattlefieldPrefab (string battlefieldID)
	{
		return BATTLEFIELD_PATH.Replace(ID_REPLACEMENT, battlefieldID) + "Battlefield";
	}

	public static string GetBattlefieldLightProbes (string battlefieldID)
	{
		return BATTLEFIELD_PATH.Replace(ID_REPLACEMENT, battlefieldID) + "LightProbes";
	}

	public static string GetBattlefieldLightmapBaseName (string battlefieldID)
	{
		return BATTLEFIELD_PATH.Replace(ID_REPLACEMENT, battlefieldID) + LIGHTMAP_NAME;
	}
}
