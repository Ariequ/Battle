using UnityEngine;
using System.Collections;

public class ActiveSelector : Selector
{
	private int currentIndex;

    override public void onInitialize(IAIContext context)
	{
		currentIndex = Mathf.Max(0, m_CurrentIndex);
	}

    override public Status update(IAIContext context)
	{
		Status s;

		if (m_CurrentChild != null)
		{
			s = m_CurrentChild.tick(context);
			
			if (s == Status.BH_FAILURE)
			{
				m_CurrentChild.onTerminate(Status.BH_ABORTED);
			}
			else
			{
				return s;
			}
		}

		currentIndex = currentIndex != m_CurrentIndex ? currentIndex + 1 : 0;

		for (; currentIndex < m_Children.Count; currentIndex++)
		{
			if (currentIndex != m_CurrentIndex)
			{
				m_CurrentChild = m_Children[currentIndex];
				s = m_CurrentChild.tick(context);
				
				if (s != Status.BH_FAILURE)
				{
					return s;
				}
			}
		}

		return Status.BH_FAILURE;
	}

	override public void onTerminate (Status status)
	{
		currentIndex = -1;
	}
}
