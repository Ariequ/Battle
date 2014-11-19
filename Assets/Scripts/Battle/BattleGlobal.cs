using UnityEngine;
using System;

using Battle;

public class BattleGlobal
{
	private static GameObject _gameController;

	private static GameSceneEffectManager _sceneEffectController;

	private static BattleValueCalculator _valueCalculator;

	private static GameElementManager _elementManager;

	private static TouchManager _touchManager;

	private static TestGameSceneInitialization _initializer;

	public static GameObject gameController
	{
		get
		{
			if (_gameController == null)
			{
				_gameController = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER);
			}
			
			return _gameController;
		}
	}


	public static GameSceneEffectManager sceneEffectController
	{
		get
		{
			if (_sceneEffectController == null)
			{
				_sceneEffectController = gameController.GetComponent<GameSceneEffectManager>();
			}
			
			return _sceneEffectController;
		}
	}

	public static BattleValueCalculator valueCalculator
	{
		get
		{
			if (_valueCalculator == null)
			{
                _valueCalculator = new BattleValueCalculator();
			}
			
			return _valueCalculator;
		}
	}

//	public static BattleStatManager statManager
//	{
//		get
//		{
//			if (_statManager == null)
//			{
//				_statManager = gameController.GetComponent<BattleStatManager>();
//			}
//			
//			return _statManager;
//		}
//	}

	public static GameElementManager elementManager
	{
		get
		{
			if (_elementManager == null)
			{
				_elementManager = gameController.GetComponent<GameElementManager>();
			}
			
			return _elementManager;
		}
	}

	public static TouchManager touchManager
	{
		get
		{
			if (_touchManager == null)
			{
				_touchManager = gameController.GetComponent<TouchManager>();
			}
			
			return _touchManager;
		}
	}

	public static TestGameSceneInitialization initializer
	{
		get
		{
			if (_initializer == null)
			{
				_initializer = gameController.GetComponent<TestGameSceneInitialization>();
			}
			
			return _initializer;
		}
	}
	
//	public static TestGameSceneInitialization initializer = gameController.GetComponent<TestGameSceneInitialization>();
		
	public static void Clear ()
	{
		_gameController = null;
		_sceneEffectController = null;
		_valueCalculator = null;
//		_statManager = null;
		_elementManager = null;
		_touchManager = null;
		_initializer = null;
	}
}

