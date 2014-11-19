using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{
	public class AttackerAscDistanceDesc : ISearchOrderLogic {

		public override String GetSearchOrderPairs ()
		{
			return SearchOrder.ParseString(SearchOrder.SearchOrderRule.ATTACKER_COUNT, SearchOrder.SearchOrderArrange.ASC, 
			                               SearchOrder.SearchOrderRule.DISTANCE, SearchOrder.SearchOrderArrange.DESC);
		}
		
		public override BattleAgentImage Order (List<BattleAgentImage> searchResult, int ptr, float distance, int attackerCount)
		{
			for (int i = 0; i < ptr; i++)
			{
				BattleAgentImage image = searchResult[i];
				
				if (image.attackers != null && image.attackers.Count > attackerCount || image.distance < distance)
				{
					return MoveForInsert(searchResult, ptr, i);
				}
			}
			
			return searchResult[ptr];
		}
	}
}