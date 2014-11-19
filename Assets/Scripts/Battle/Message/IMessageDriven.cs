using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{
	public interface IMessageDriven {
		
		String[] GetMessageTypes ();
		
		void OnMessageArrived (MessageContext context);
	}
}
