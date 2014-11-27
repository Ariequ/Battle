using UnityEngine;
using System.Collections;

public class Parallel : Composite
{
	public enum Policy
	{
		RequireOne,
		RequireAll,
	};

	protected Policy m_eSuccessPolicy;
	protected Policy m_eFailurePolicy;

	public Parallel ()
	{
	}
	
	public void setSussessPlicy(int forSuccess)
	{
		m_eSuccessPolicy = (Policy)forSuccess;
	}

	public void setFailurePolicy(int forFailure)
	{
		m_eFailurePolicy = (Policy)forFailure;
	}

    override public Status update (IAIContext context)
	{	
		int iSuccessCount = 0, iFailureCount = 0;
		
		for (int i=0; i<this.m_Children.Count; i++) {
			Behavior b = m_Children [i];
			if (!b.isTerminated ()) {
				b.tick (context);
			}
			
			if (b.getStatus () == Status.BH_SUCCESS) {
				++iSuccessCount;
				if (m_eSuccessPolicy == Policy.RequireOne) {
					return Status.BH_SUCCESS;
				}
			}
			
			if (b.getStatus () == Status.BH_FAILURE) {
				++iFailureCount;
				if (m_eFailurePolicy == Policy.RequireOne) {
					return Status.BH_FAILURE;
				}
			}
		}
		
		if (m_eFailurePolicy == Policy.RequireAll && iFailureCount == m_Children.Count) {
			return Status.BH_FAILURE;
		}

		if (m_eSuccessPolicy == Policy.RequireAll && iSuccessCount == m_Children.Count) {
			return Status.BH_SUCCESS;
		}
		
		return Status.BH_RUNNING;
	}
	
	override public void onTerminate (Status status)
	{
		foreach (Behavior b in m_Children) {
			if (b.isRunning ()) {
				b.abort ();
			}
		}
	}
}
