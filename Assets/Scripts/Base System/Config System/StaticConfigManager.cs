using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Hero.Config;
using Hero.Message;
using Thrift.Protocol;

public class StaticConfigManager {

	private static StaticConfigManager instance;
	
	public static StaticConfigManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new StaticConfigManager();
			}
			
			return instance;
		}
	}

	private CharacterLevelsConfig characterLevelsConfig;

	private ConsumerItemsConfig itemsConfig;

	private ExchangesConfig exchangesConfig;

	private FuncConfig funcConfig;

	private LimitConfg limitConfig;

	private LimitFuncScenesConfig limitFuncScenesConfig;

	private HerosConfig herosConfig;

	private SoldiersConfig soldiersConfig;

	private Hero.Config.MessageConfig messageConfig;

	public CharacterLevelsConfig CharacterLevelsConfig
	{
		get
		{
			return this.characterLevelsConfig;
		}
	}

	public CharacterLevelConfig GetCharacterLevelConfigById(short id)
	{
		return characterLevelsConfig.LevelMap [id];
	}

	public ConsumerItemsConfig ConsumerItemsConfig
	{
		get
		{
			return this.itemsConfig;
		}
	}

	public ConsumerItemConfig GetConsumerItemConfigById(int id)
	{
		return itemsConfig.ItemConfigMap [id];
	}

	public ExchangesConfig ExchangesConfig
	{
		get
		{
			return this.exchangesConfig;
		}
	}

	public ExchangeConfig GetExchangeConfigById(int id)
	{
		return exchangesConfig.ExchangeConfigMap [id];
	}

	public FuncConfig FuncConfig
	{
		get
		{
			return this.funcConfig;
		}
	}

	public FuncGroup GetFuncGroupById(int id)
	{
		return funcConfig.FuncMap [id];
	}

	public LimitConfg LimitConfg
	{
		get
		{
			return this.limitConfig;
		}
	}

	public LimitGroup GetLimitGroupById(int id)
	{
		return limitConfig.LimitMap [id];
	}

	public LimitFuncScenesConfig LimitFuncScenesConfig
	{
		get
		{
			return this.limitFuncScenesConfig;
		}
	}

	public LimitFuncSceneConfig GetLimitFuncSceneConfigById(int id)
	{
		return limitFuncScenesConfig.ScenenConfigMap[id];
	}

	public HerosConfig HerosConfig
	{
		get
		{
			return this.herosConfig;
		}
	}

	public HeroConfig GetHeroConfigById(int id)
	{
		return herosConfig.CardConfigMap[id];
	}
	
	public Dictionary<int, HeroConfig> GetHeroConfigByGroup(int gid)
	{
		return null;			// TODO
	}

	public SoldiersConfig SoldiersConfig
	{
		get
		{
			return this.soldiersConfig;
		}
	}

	public SoldierConfig GetSoldierConfigById(int id)
	{
		return soldiersConfig.CardConfigMap[id];
	}

	public Dictionary<int, SoldierConfig> GetSoldierConfigByGroup(int gid)
	{
		return null;			// TODO
	}
	
	public Hero.Config.MessageConfig MessageConfig
	{
		get
		{
			return this.messageConfig;
		}
	}

	public Dictionary<int, string> GetLocalMessageConfig (string location)
	{
		return messageConfig.MessageMap[location];
	}

	public void LoadConfig ()
	{
		Dictionary<string, object> loadConfigs = new Dictionary<string, object>();

		loadConfigs.Add (StaticConfigPath.CHARACTER_LEVEL_CONFIG, new CharacterLevelsConfig());
		loadConfigs.Add (StaticConfigPath.CONSUMER_ITEM_CONFIG, new ConsumerItemsConfig());
		loadConfigs.Add (StaticConfigPath.EXCHANGE_CONFIG, new ExchangesConfig());
		loadConfigs.Add (StaticConfigPath.FUNC_CONFIG, new FuncConfig());
		loadConfigs.Add (StaticConfigPath.LIMIT_CONFIG, new LimitConfg());
		loadConfigs.Add (StaticConfigPath.LIMIT_FUNC_SCENE_CONFIG, new LimitFuncScenesConfig());
		loadConfigs.Add (StaticConfigPath.HERO_CONFIG, new HerosConfig());
		loadConfigs.Add (StaticConfigPath.SOLDIER_CONFIG, new SoldiersConfig());
		loadConfigs.Add (StaticConfigPath.MESSAGE_CONFIG, new Hero.Config.MessageConfig());

		foreach (KeyValuePair<string, object> loadConfig in loadConfigs)
		{
			using (FileStream fs = File.Open(Application.streamingAssetsPath + loadConfig.Key, FileMode.Open))
			{
				byte[] bytes = new byte[fs.Length];
				fs.Read(bytes, 0, bytes.Length);
				fs.Close();

				TBase tbaseObject = loadConfig.Value as TBase;
				ThriftSerialize.DeSerialize(tbaseObject, bytes);

//				Debug.Log("Load " + tbaseObject.GetType().ToString() + " Config");
			}
		}

		this.characterLevelsConfig = loadConfigs [StaticConfigPath.CHARACTER_LEVEL_CONFIG] as CharacterLevelsConfig;
		this.itemsConfig = loadConfigs [StaticConfigPath.CONSUMER_ITEM_CONFIG] as ConsumerItemsConfig;
		this.exchangesConfig = loadConfigs [StaticConfigPath.EXCHANGE_CONFIG] as ExchangesConfig;
		this.funcConfig = loadConfigs [StaticConfigPath.FUNC_CONFIG] as FuncConfig;
		this.limitConfig = loadConfigs [StaticConfigPath.LIMIT_CONFIG] as LimitConfg;
		this.limitFuncScenesConfig = loadConfigs [StaticConfigPath.LIMIT_FUNC_SCENE_CONFIG] as LimitFuncScenesConfig;
		this.herosConfig = loadConfigs [StaticConfigPath.HERO_CONFIG] as HerosConfig;
		this.soldiersConfig = loadConfigs [StaticConfigPath.SOLDIER_CONFIG] as SoldiersConfig;
		this.messageConfig = loadConfigs [StaticConfigPath.MESSAGE_CONFIG] as Hero.Config.MessageConfig;
	}
}
