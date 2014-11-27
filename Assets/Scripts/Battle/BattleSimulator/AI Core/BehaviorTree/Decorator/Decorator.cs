using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Decorator : Behavior
{
	protected Behavior m_pchild;

	public void setChild (Behavior child)
	{
		m_pchild = child;
	}
}
