using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Faction {Self, Opponent, All}

public enum Force {Light, Dark}

public class GameGlobal
{
	public const float BATTLE_SIMULATION_STEP = 1f / 30f;

	public static string lastLegendSceneName;

	private static string loadingSceneName;

	public static void LoadSceneByName(string sceneName)
	{
		loadingSceneName = sceneName;
		Application.LoadLevel (SceneNameConst.LOADING_SCENE);
	}

	public static string LoadingSceneName
	{
		get
		{
			return loadingSceneName;
		}
	}
}
