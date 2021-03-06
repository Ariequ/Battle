﻿using UnityEngine;
using System.Collections;

public class RepeatDecorator : Decorator
{
	protected int m_iLimit;
	protected int m_iCounter;

	public RepeatDecorator ()
	{
		m_iLimit = 5;
	}
	 
    override public void onInitialize (IAIContext context)
	{
		m_iCounter = 0;
		m_iLimit = int.Parse(this.getParam()["repeatCount"]);
	}
	
    override public Status update (IAIContext context)
	{
		for (;;) {
			Status s =  this.m_pchild.tick(context);

			if (s == Status.BH_RUNNING)
			{
				break;
			}

			if (s == Status.BH_FAILURE)
			{
				return Status.BH_FAILURE;
			}

			if (++m_iCounter == m_iLimit)
			{
				return Status.BH_SUCCESS;
			}

			this.m_pchild.reset();
		}
		return Status.BH_RUNNING;
	}
}
