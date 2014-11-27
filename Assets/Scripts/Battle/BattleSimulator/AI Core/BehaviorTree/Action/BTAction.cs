using UnityEngine;
using System.Collections;

public class BTAction : Behavior
{
    override public Status update(IAIContext context)
    {
        return m_eStatus;
    }
}
