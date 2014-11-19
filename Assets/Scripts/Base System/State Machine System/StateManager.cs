using System;
using System.Collections;
using System.Collections.Generic;


namespace StateMachine
{
	public class StateManager
	{
		protected string name;

		protected Dictionary<string, State> stateDic = new Dictionary<string, State>();

		protected State currentState;

		public StateManager (string name)
		{
			this.name = name;
		}

		public string Name
		{
			get
			{
                return name;
            }
        }
        
        public virtual void InitializeStateFlow()
		{
		}

		public virtual void UpdateState()
		{
			Transition transition = currentState.CheckTransitions();

			if (transition != null)
			{
				State prevState = currentState;
				Context context = prevState.Exit();

				currentState = transition.Translate();
				currentState.Enter(context);
			}
		}

		public virtual object DoAction(string key, object param = null)
		{
			IAction action = currentState.GetAction(key);
			object result = action.Execute(param);

			UpdateState();

			return result;
		}
	}
}

