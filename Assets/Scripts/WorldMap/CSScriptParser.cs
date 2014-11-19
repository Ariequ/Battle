using UnityEngine;

using System;
using System.Collections.Generic;

public class CSScriptParser : MonoBehaviour
{
	private Dictionary<string, Dictionary<string, string>> scriptsInfoDic;

	public void initParser (Dictionary<string, Dictionary<string, string>> scriptsInfoDic)
	{
		this.scriptsInfoDic = scriptsInfoDic;

		foreach (string scriptName in scriptsInfoDic.Keys)
		{
			parse(scriptName);
		}
	}

	private void parse (string scriptName)
	{
		MonoBehaviour script = GetComponent(scriptName) as MonoBehaviour;
		script.enabled = false;

		Dictionary<string, string> valueDic = scriptsInfoDic[scriptName];

		switch(scriptName)
		{
			case "CustomSmoothFollow":
				parseSmoothFollow(script as CustomSmoothFollow, valueDic);
				break;
		}

		script.enabled = true;
	}

	private void parseSmoothFollow (CustomSmoothFollow smoothFollow, Dictionary<string, string> valueDic)
	{
		foreach (KeyValuePair<string, string> pair in valueDic)
		{
			switch(pair.Key)
			{
			case "target":
				smoothFollow.target = null;
				break;
			case "distance":
				smoothFollow.distance = Convert.ToSingle(pair.Value);
				break;
			case "height":
				smoothFollow.height = Convert.ToSingle(pair.Value);
				break;
			case "heightDamping":
				smoothFollow.heightDamping = Convert.ToSingle(pair.Value);
				break;
			case "rotationDamping":
				smoothFollow.rotationDamping = Convert.ToSingle(pair.Value);
				break;
			}
		}
	}
}

