using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{
	public class AttackOccurSensorAnalyzer : AbstractSensorAnalyzer
	{
		public AttackOccurSensorAnalyzer (BattleAgentManager battleAgentManager) : base (battleAgentManager)
		{
			
		}

		public override String GetMessageType ()
		{
			return Messages.ATTACK_OCCUR;
		}

		public override void DoAnalyze (IGameSensor sensor, MessageContext context)
		{
			String name = ((BattleAgent)sensor.Agent).Name;
			String enemyName = (String) context.GetConetextValue(AttackParamKey.CURRENT_ENEMY);
			BattleAgent enemy = battleAgentManager.GetCharacterByName(enemyName);
			float distance = battleAgentManager.GetDistanceForTarget(enemyName, name);
			List<String> attackers = (List<String>) battleAgentManager.GetAttackersByName(name);

			sensor.FeedbackAttack(SensorFeedbackType.ATTACKED, enemy, distance, attackers);
		}
	}
}
