using System;
namespace StateMachine
{
	public interface IAction
	{
		object Execute(object param = null);
	}
}

