using UnityEngine;
using System.Collections;

public class BattleLabelController : MonoBehaviour
{
	[HideInInspector]
	public UILabel uiLabel;

	[HideInInspector]
	public UITweener uiTweener;

	[HideInInspector]
	public ElementController elementController;

	void Awake ()
	{
		//Store all useful components' references.
		uiLabel = GetComponent<UILabel>();

		uiTweener = GetComponent<UITweener>();

		elementController = GetComponent<ElementController>();
	}
}

