using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TouchHandlerType
{
	Press,
	Stay,
	Release
}

public class TouchManager : MonoBehaviour 
{
	public float updateInterval = 0.1f;

	//For convenience concern, store all kinds of handler logic in a single AbstraclerTouchHandler.
	//If here is any problem of performance, you can break this into several different dictionaries.
	private List<AbstractTouchHandler> touchHandlers = new List<AbstractTouchHandler>();

	private float timer;

	void Awake () 
	{
		isDragging = false;

		timer = 0f;
	}
	
	void Update () 
	{
		timer += BattleTime.deltaTime;

		if (timer >= updateInterval)
		{
			timer = 0f;
			
			lock(touchHandlers)
			{
				if (Input.GetMouseButton(0))
				{
					if (!isDragging)
					{
						isDragging = true;
						
						foreach(AbstractTouchHandler touchHandler in touchHandlers)
						{
							if (touchHandler.isActive)
							{
								touchHandler.OnPress();
							}
						}
					}

					foreach(AbstractTouchHandler touchHandler in touchHandlers)
					{
						if (touchHandler.isActive)
						{
							touchHandler.OnStay();
						}
					}
				}
				else if (isDragging)
				{
					isDragging = false;

					foreach(AbstractTouchHandler touchHandler in touchHandlers)
					{
						if (touchHandler.isActive)
						{
							touchHandler.OnRelease();
						}
					}
				}
			}
		}
	}

	public void AddTouchHandler(AbstractTouchHandler handler)
	{
		touchHandlers.Add (handler);
	}

	public void RemoveTouchHandler(AbstractTouchHandler handler)
	{
		touchHandlers.Remove (handler);
	}

	public bool isDragging { get; private set;}
}
