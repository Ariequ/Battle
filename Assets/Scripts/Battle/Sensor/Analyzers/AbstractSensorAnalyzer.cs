using System;
using Battle;

namespace Battle
{
	public abstract class AbstractSensorAnalyzer : ISensorAnalyzer
	{
		protected BattleAgentManager battleAgentManager;

		public AbstractSensorAnalyzer (BattleAgentManager battleAgentManager)
		{
			this.battleAgentManager = battleAgentManager;
		}

		public abstract String GetMessageType ();
		
		public abstract void DoAnalyze (IGameSensor sensor, MessageContext context);
	}
}

