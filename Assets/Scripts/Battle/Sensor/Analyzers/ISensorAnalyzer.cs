using System;

namespace Battle
{
	public interface ISensorAnalyzer
	{
		String GetMessageType ();

		void DoAnalyze (IGameSensor sensor, MessageContext context);
	}
}

