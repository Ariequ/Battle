using UnityEngine;
using System.Collections;

public enum ElementType
{
	Effect,
	FlySoldier,
	BattleLabel,
	TombStone
}

public class ElementController : MonoBehaviour
{
	[HideInInspector]
	public int metaID;

	public ElementType elementType;

	public float recycleDelay = 3f;

	public int cacheNum = 5;

	public bool reusable = true;

	public float preserveDuration = 5f;

	private float _recycleTimer;

	protected virtual void Awake ()
	{
		_recycleTimer = 0f;
	}

	protected virtual void Update ()
	{
		if (recycleDelay < 0)
		{
			return;
		}

		_recycleTimer += BattleTime.deltaTime;
		
		if (_recycleTimer >= recycleDelay)
		{
			Recycle();
		}
	}

	/// <summary>
	/// Recycle this instance. 
	/// Override this method and call the base method in the end of your overriding method.
	/// </summary>
	public virtual void Recycle()
	{
		_recycleTimer = 0f;

		if (this.reusable) 
		{
			BattleGlobal.elementManager.RecycleElement (this);
		} 
		else 
		{
			Destroy(gameObject);
		}
	}
}

