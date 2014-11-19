using UnityEngine;
using System.Collections;

public class AbstractTouchHandler : MonoBehaviour
{
	internal bool isActive;

	protected void AddTouchHandler()
	{
		BattleGlobal.touchManager.AddTouchHandler(this);
	}

	protected void RemoveTouchHandler()
	{
		BattleGlobal.touchManager.RemoveTouchHandler(this);
	}

	public virtual void OnPress()
	{

	}

	public virtual void OnStay()
	{
		
	}

	public virtual void OnRelease()
	{
		
	}
}