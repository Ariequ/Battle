using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{
	public abstract class ISearchOrderLogic {

		public abstract String GetSearchOrderPairs ();

		public abstract BattleAgentImage Order (List<BattleAgentImage> searchResult, int ptr, float distance, int attackerCount);

		protected static BattleAgentImage MoveForInsert (List<BattleAgentImage> searchResult, int ptr, int index)
		{
			for (int i = ptr; i > index; i--)
			{
				searchResult[i] = searchResult[i - 1];
			}

			return searchResult[index];
		}
	}
}