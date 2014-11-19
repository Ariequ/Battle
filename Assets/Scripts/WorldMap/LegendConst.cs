using UnityEngine;
using System.Collections;

public class LegendConst 
{
	public const float CULLING_INTERVAL = 1f;
	public const float CULLING_CELL_SIZE = 25f;
	public const int CULLING_CELL_COUNT = 30;
	public const int SEARCH_RANGE = 1;

	public const float MIN_MOUSEMOVE_DISTANCE = 10f;
	public const float CAMERA_MOVE_SPEED = 10f;

	public const string BLOCK_NAME = "area_";
	public const string LIGHTMAP_NAME = "LightmapFar-";

	public const string MAP_BLOCKS_CONFIG = "MapBlocksConfig";
	public const string MAP_BLOCKS_MODEL_CONFIG = "MapBlockModelsConfig";
	public const string BUILDINGS_CONFIG = "BuildingsConfig";
	public const string MONSTERS_CONFIG = "MonstersConfig";
	public const string TREASURES_CONFIG = "TreasuresConfig";
	public const string MAP_GLOBAL_CONFIG = "MapGlobalConfig";
	public static string[] CONFIG_LIST = new string[]
	{
		MAP_BLOCKS_CONFIG, 
		MAP_BLOCKS_MODEL_CONFIG,
		BUILDINGS_CONFIG,
		MONSTERS_CONFIG,
		TREASURES_CONFIG,
		MAP_GLOBAL_CONFIG,
	};
}
