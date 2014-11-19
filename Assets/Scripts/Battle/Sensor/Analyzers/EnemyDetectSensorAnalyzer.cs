using UnityEngine;
using System;

namespace Battle
{
	public class EnemyDetectSensorAnalyzer : AbstractSensorAnalyzer
	{
		public EnemyDetectSensorAnalyzer (BattleAgentManager battleAgentManager) : base (battleAgentManager)
		{
			
		}

		public override String GetMessageType ()
		{
			return Messages.DISTANCE_DETECT;
		}
		
		public override void DoAnalyze (IGameSensor sensor, MessageContext context)
		{
			String enemyName = (String) context.GetConetextValue(AttackParamKey.CURRENT_ENEMY);
			BattleAgent enemy = battleAgentManager.GetCharacterByName(enemyName);
			float distance = (float) context.GetConetextValue(AttackParamKey.ENEMY_DISTANCE);
			
			sensor.FeedbackDistance(SensorFeedbackType.DISTANCE_DETECT, enemy, distance);
		}
	}
}
