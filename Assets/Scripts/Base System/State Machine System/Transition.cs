using System;

namespace StateMachine
{
	public class Transition
	{
		protected ICondition condition;

		protected State nextState;

		public Transition (State state)
		{
			nextState = state;
		}

		public Context Context { get; set; }


		public bool CheckCondition()
		{
			return condition.Determine();
		}

		public State Translate()
		{
			return nextState;
		}
	}
}