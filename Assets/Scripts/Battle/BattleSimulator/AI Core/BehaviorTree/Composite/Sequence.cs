using UnityEngine;
using System.Collections;

public class Sequence: Composite
{
    private Behavior m_CurrentChild;

    override public void onInitialize (IAIContext context)
    {
        m_CurrentChild = m_Children [0];
    }
    
    override public Status update (IAIContext context)
    {
        // Keep going until a child behavior says it's running.
        for (;;)
        {
            Status s = m_CurrentChild.tick (context);
            
            // If the child fails, or keeps running, do the same.
            if (s != Status.BH_SUCCESS)
            {
                return s;
            }

            int currentIndex = m_Children.IndexOf (m_CurrentChild);
            // Hit the end of the array, job done!
            if (currentIndex == m_Children.Count - 1)
            {
//				m_CurrentChild = m_Children [0];
                return Status.BH_SUCCESS;
            }
            else
            {
                m_CurrentChild = m_Children [currentIndex + 1];
            }
        }
    }
}
