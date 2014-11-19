using UnityEngine;
using System.Collections;

public abstract class ISkillAdditionEffectController : MonoBehaviour 
{
	protected ElementController elementController;

	void Awake () 
	{
		this.elementController = GetComponent<ElementController>();

		OnAwake ();
	}

	public abstract void StartEffect ();

	protected virtual void OnAwake ()
	{
	}
}
