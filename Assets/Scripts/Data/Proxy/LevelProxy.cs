using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;

public class LevelProxy : Proxy
{
	new public const string NAME = "LevelProxy";

	public LevelProxy () : base(NAME)
	{

	}

	public override void OnRemove ()
	{

	}

	public LevelVO GetLevelVO (int levelID)
	{
		foreach (LevelVO levelVO in MockServer.levelVOList)
		{
			if (levelVO.Meta.id == levelID)
			{
				return levelVO;
			}
		}
		
		return null;
	}
}

