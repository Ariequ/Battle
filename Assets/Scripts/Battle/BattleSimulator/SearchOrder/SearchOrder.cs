using UnityEngine;
using System;
using System.Collections;

namespace Battle
{
	public class SearchOrder {
		
		public enum SearchOrderRule
		{
			ATTACKER_COUNT = 10,
			DISTANCE = 20
		}
		
		public enum SearchOrderArrange
		{
			ASC = 3,
			DESC = 4
		}
		
		public SearchOrderRule primaryRule;
		
		public SearchOrderArrange primaryArrange;
		
		public SearchOrderRule secondaryRule;
		
		public SearchOrderArrange secondaryArrange;
		
		private String identity;
		
		public static SearchOrder CreateSearchOrder (SearchOrderRule primaryRule, SearchOrderArrange primaryArrange, 
		                                             SearchOrderRule secondaryRule, SearchOrderArrange secondaryArrange)
		{
			SearchOrder searchOrder = new SearchOrder();
			searchOrder.primaryRule = primaryRule;
			searchOrder.primaryArrange = primaryArrange;
			searchOrder.secondaryRule = secondaryRule;
			searchOrder.secondaryArrange = primaryArrange;
			
			searchOrder.identity = ParseString(primaryRule, primaryArrange, secondaryRule, secondaryArrange);
			
			return searchOrder;
		}
		
		public static String ParseString (SearchOrderRule primaryRule, SearchOrderArrange primaryArrange, 
		                                  SearchOrderRule secondaryRule, SearchOrderArrange secondaryArrange)
		{
			return ((int)primaryRule + (int)primaryArrange).ToString() + ((int)secondaryRule + (int)secondaryArrange).ToString();
		}
		
		public override String ToString ()
		{
			return this.identity;
		}
	}

}