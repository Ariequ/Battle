#pragma strict

import System;
import System.Collections.Generic;

class JSScriptParser extends MonoBehaviour
{
	private var scriptsInfoDic : Dictionary.<String, Dictionary.<String, String> >;

	public function initParser (scriptsInfoDic : Dictionary.<String, Dictionary.<String, String> >) : void
	{
		this.scriptsInfoDic = scriptsInfoDic;
		
		for (var imageEffectName : String in scriptsInfoDic.Keys)
		{
			parse(imageEffectName);
		}
	}
	
	public function parse(imageEffectName : String) : void
	{
		var imageEffect : MonoBehaviour = gameObject.AddComponent(imageEffectName) as MonoBehaviour;
		imageEffect.enabled = false;
		
		var valueDic : Dictionary.<String, String> = scriptsInfoDic[imageEffectName];
		
		switch (imageEffectName)
		{
			case "GlobalFog":
				parseGlobalFog(imageEffect as GlobalFog, valueDic);
				break;
			case "Vignetting":
				parseVignetting(imageEffect as Vignetting, valueDic);
				break;
			case "FastBloom":
				parseFastBloom(imageEffect as FastBloom, valueDic);
				break;
		}
		
		imageEffect.enabled = true;
	}
	private function parseGlobalFog (imageEffect : GlobalFog, valueDic : Dictionary.<String, String> ) : void
	{
		for (var pair : KeyValuePair.<String, String> in valueDic)
		{
			switch(pair.Key)
			{
				case "fogMode":
					imageEffect.fogMode = Enum.Parse(GlobalFog.FogMode, pair.Value);
					break;
				case "startDistance":
					imageEffect.startDistance = Convert.ToSingle(pair.Value);
					break;
				case "globalDensity":
					imageEffect.globalDensity = Convert.ToSingle(pair.Value);
					break;
				case "heightScale":
					imageEffect.heightScale = Convert.ToSingle(pair.Value);
					break;
				case "height":
					imageEffect.height = Convert.ToSingle(pair.Value);
					break;
				case "globalFogColor":
					imageEffect.globalFogColor = getColorFromStr(pair.Value);
					break;
				case "fogShader":
					imageEffect.fogShader = Shader.Find(pair.Value);
					break;
			}
		}
	}
	
	private function parseVignetting (imageEffect : Vignetting, valueDic : Dictionary.<String, String> ) : void
	{
		for (var pair : KeyValuePair.<String, String> in valueDic)
		{
			switch(pair.Key)
			{
				case "mode":
					imageEffect.mode = Enum.Parse(Vignetting.AberrationMode, pair.Value);
					break;
				case "intensity":
					imageEffect.intensity = Convert.ToSingle(pair.Value);
					break;
				case "chromaticAberration":
					imageEffect.chromaticAberration = Convert.ToSingle(pair.Value);
					break;
				case "axialAberration":
					imageEffect.axialAberration = Convert.ToSingle(pair.Value);
					break;
				case "blur":
					imageEffect.blur = Convert.ToSingle(pair.Value);
					break;
				case "blurSpread":
					imageEffect.blurSpread = Convert.ToSingle(pair.Value);
					break;
				case "luminanceDependency":
					imageEffect.luminanceDependency = Convert.ToSingle(pair.Value);
					break;
				case "vignetteShader":
					imageEffect.vignetteShader = Shader.Find(pair.Value);
					break;
				case "separableBlurShader":
					imageEffect.separableBlurShader = Shader.Find(pair.Value);
					break;
				case "chromAberrationShader":
					imageEffect.chromAberrationShader = Shader.Find(pair.Value);
					break;
			}
		}
	}
	
	private function parseFastBloom (imageEffect : FastBloom, valueDic : Dictionary.<String, String> ) : void
	{
		for (var pair : KeyValuePair.<String, String> in valueDic)
		{
			switch(pair.Key)
			{
				case "threshhold":
					imageEffect.threshhold = Convert.ToSingle(pair.Value);
					break;
				case "intensity":
					imageEffect.intensity = Convert.ToSingle(pair.Value);
					break;
				case "blurSize":
					imageEffect.blurSize = Convert.ToSingle(pair.Value);
					break;
				case "resolution":
					imageEffect.resolution = Enum.Parse(FastBloom.Resolution, pair.Value);
					break;
				case "blurIterations":
					imageEffect.blurIterations = Convert.ToSingle(pair.Value);
					break;
				case "blurType":
					imageEffect.blurType = Enum.Parse(FastBloom.BlurType, pair.Value);
					break;
				case "fastBloomShader":
					imageEffect.fastBloomShader = Shader.Find(pair.Value);
					break;
			}
		}
	}
	
	private function getColorFromStr (colorStr : String) : Color
	{
		var colorArray : Array = colorStr.Split(','[0]);
		var color : Color = new Color();
		
		for (var i : int = 0; i < colorArray.length; i++)
		{
			var colorValue : float = Convert.ToSingle(colorArray[i]);
			color[i] = colorValue;
		}
		
		return color;
	}
}