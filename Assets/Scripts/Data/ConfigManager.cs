using UnityEngine;

using System;
using System.Collections;
using System.IO;
using System.Text;

using Thrift.Protocol;
using Hero.Message;

public enum ConfigType
{
	//TODO: add sub config types.
	None
}

public class ConfigManager
{
//	public const 

	public ConfigManager ()
	{
	}

	public TBase GetConfig(string configName, Type configType) 
	{
		string path = getPath(configName);
		using(FileStream fs = new FileStream(path, FileMode.Open)){
			long len = fs.Length;
			byte[] bytes = new byte[(int)len];
			fs.Read(bytes, 0, bytes.Length);
			
			TBase config = Activator.CreateInstance(configType) as TBase;
			ThriftSerialize.DeSerialize(config, bytes);

			return config as TBase;
		}
	}
	
	public T GetConfig<T>(string configName) where T : TBase
	{
		string path = getPath(configName);
		using(FileStream fs = new FileStream(path, FileMode.Open)){
			long len = fs.Length;
			byte[] bytes = new byte[(int)len];
			fs.Read(bytes, 0, bytes.Length);
			
			TBase config = Activator.CreateInstance<T>() as TBase;
			ThriftSerialize.DeSerialize(config, bytes);
			
			return (T)config;
		}
	}

	private string getPath(string configName)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(Application.dataPath);
		sb.Append("/Resources/Configs/Files/");
		sb.Append(configName);
		sb.Append(".bytes");
		return sb.ToString();
	}
}

