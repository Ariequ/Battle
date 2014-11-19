using UnityEngine;
using System.Collections;

public class PriorityParallel : Parallel
{
	public PriorityParallel ()
	{
		m_eSuccessPolicy = Policy.RequireOne;
	}

    override public Status update (IAIContext context)
	{	
		Behavior mostValueChild = getMostValueChild ();
		if (mostValueChild != null) {
			return mostValueChild.tick (context);
		}

		return Status.BH_FAILURE;
	}

	private Behavior getMostValueChild ()
	{
		PriorityDecorator choosen = (PriorityDecorator)this.m_Children [0];
		for (int i = 1; i < this.m_Children.Count; i ++) {
			PriorityDecorator current = (PriorityDecorator)this.m_Children [i];
			if (current.getPriority () < choosen.getPriority ()) {
				choosen = current;
			}
		}

		if (checkWeight (choosen.getWeight ())) {
			return choosen;
		}

		return null;
	}

	private bool checkWeight (int weight)
	{
		return true;
	}

}

