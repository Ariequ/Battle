using System;
using System.Collections;
using System.Collections.Generic;

namespace StateMachine
{
	public class State
	{
		protected string name;

		protected Context context;

		private Dictionary<string, IAction> actionDic;

		private List<Transition> transitionList;

		public State (string name)
		{
			this.name = name;

			context = new Context();
			actionDic = new Dictionary<string, IAction>();
			transitionList = new List<Transition>();
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		internal void Enter(Context context)
		{
			this.context = context;

			OnEnter();
		}

		internal Context Exit()
		{
			OnExit();

			return context;
		}

		protected virtual void OnEnter() {}
		
		protected virtual void OnExit() {}

		internal Transition CheckTransitions()
		{
			int count = transitionList.Count;
			for (int i = 0; i < count; ++i)
			{
				Transition transition = transitionList[i];
				if (transition.CheckCondition())
					return transition;
			}
			return null;
		}

		internal void AddAction(string key, IAction action)
		{
			actionDic[key] = action;
		}

		public IAction GetAction(string key)
		{
			IAction action = null;
			actionDic.TryGetValue(key, out action);
			return action;
		}

		internal void AddTransition(Transition transition)
		{
			transitionList.Add(transition);
		}
	}
}

