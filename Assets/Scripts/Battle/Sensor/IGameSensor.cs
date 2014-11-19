using Battle;
using System.Collections.Generic;

namespace Battle
{
	public interface IGameSensor
	{
		object Agent { get;}
		
		void FeedbackAttack(SensorFeedbackType type, BattleAgent agent, float distance, List<string> attackers);
		
		void FeedbackDistance(SensorFeedbackType type, BattleAgent agent, float distance);
	}
}
