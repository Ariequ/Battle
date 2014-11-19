using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{
	public class AttackLockedSensorAnalyzer : AbstractSensorAnalyzer
	{
		public AttackLockedSensorAnalyzer (BattleAgentManager battleAgentManager) : base (battleAgentManager)
		{

		}

		public override String GetMessageType ()
		{
			return Messages.ATTACK_LOCKED;
		}

		public override void DoAnalyze (IGameSensor sensor, MessageContext context)
		{
			String enemyName = (String) context.GetConetextValue(AttackParamKey.CURRENT_ENEMY);
			BattleAgent enemy = battleAgentManager.GetCharacterByName(enemyName);
			float distance = battleAgentManager.GetDistanceForTarget(enemyName, ((BattleAgent)sensor.Agent).Name);
			List<String> attackers = (List<String>) context.GetConetextValue(AttackParamKey.LOCKED_ATTACKERS);

			sensor.FeedbackAttack(SensorFeedbackType.ATTACK_LOCKED, enemy, distance, attackers);
		}
	}
}

