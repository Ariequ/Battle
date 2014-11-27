using UnityEngine;
using System.Collections;

public class PrioritySelector : Selector
{
	public PrioritySelector ()
	{
	}
	
    override public Status update (IAIContext context)
	{	
		Behavior mostValueChild = getMostValueChild ();
		if (mostValueChild != null) {
			Status status = mostValueChild.tick (context);
			return status;
		}

		return Status.BH_FAILURE;
	}
	
	private Behavior getMostValueChild ()
	{
		Behavior choosen = (Behavior)this.m_Children [0];
		for (int i = 1; i < this.m_Children.Count; i ++) {
			Behavior current = (Behavior)this.m_Children [i];
			if (int.Parse (current.getParam () ["priority"]) < int.Parse (choosen.getParam () ["priority"])) {
				choosen = current;
			}
		}
		
		if (checkWeight (int.Parse (choosen.getParam () ["weight"]))) {
			return choosen;
		}
		
		return null;
	}
	
	private bool checkWeight (int weight)
	{
		return true;
	}
	
}

