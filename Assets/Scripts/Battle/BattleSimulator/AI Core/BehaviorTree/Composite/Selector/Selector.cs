using UnityEngine;
using System.Collections;

public class Selector : Composite 
{
	protected int m_CurrentIndex;
	protected Behavior m_CurrentChild;

    override public void onInitialize(IAIContext context)
	{
		m_CurrentIndex = 0;
		m_CurrentChild = m_Children[m_CurrentIndex];
	}
	
    override public Status update(IAIContext context)
	{
		Status s = m_CurrentChild.tick(context);
		
		if (s == Status.BH_FAILURE)
		{
			for (m_CurrentIndex += 1; m_CurrentIndex < m_Children.Count; m_CurrentIndex++)
			{
				m_CurrentChild = m_Children[m_CurrentIndex];
				s = m_CurrentChild.tick(context);
				
				if (s != Status.BH_FAILURE)
				{
					return s;
				}
			}
		}
		
		return s;
	}

	override public void onTerminate (Status status)
	{
		m_CurrentIndex = -1;
		m_CurrentChild = null;
	}
}
