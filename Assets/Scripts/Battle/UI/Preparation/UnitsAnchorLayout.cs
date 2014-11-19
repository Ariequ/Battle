using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsAnchorLayout : UIDragDropLayout {

	public Transform sendBackTarget;

	private UIDragDropLayout sendBackLayout;

	void Start () 
	{
		if (this.sendBackTarget != null)
		{
			this.sendBackLayout = sendBackTarget.GetComponent<UIDragDropLayout>();
		}
	}
	
	override public void Layout (Transform item)
	{
		AbstractAnchorInfo[] currentAnchorInfos = GetComponentsInChildren<AbstractAnchorInfo>();

		foreach (AbstractAnchorInfo currentAnchorInfo in currentAnchorInfos)
		{
			if (!ReferenceEquals(currentAnchorInfo.transform, item))
			{
				Transform currentUnit = currentAnchorInfo.transform;
				
				if (sendBackTarget != null)
				{
					currentUnit.parent = sendBackTarget;
					
					if (this.sendBackLayout != null)
					{
						sendBackLayout.Layout(currentUnit);
					}
				}
				else
				{
					Destroy(currentUnit.gameObject);
				}

				break;
			}
		}

		item.localPosition = Vector3.zero;
	}
}
