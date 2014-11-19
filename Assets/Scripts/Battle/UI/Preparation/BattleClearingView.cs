using UnityEngine;
using System.Collections;

public class BattleClearingView : BasePopupView 
{
	public UIButton closeButton;

	public UILabel defeatLabel;

	public UILabel victoryLabel;

	public UILabel defeatSubtitle;
	
	public UILabel victorySubtitle;

	public void UpdateUI (LevelVO levelVO, bool victory)
	{
		closeButton.collider.enabled = true;

		defeatLabel.gameObject.SetActive (!victory);
		defeatSubtitle.gameObject.SetActive (!victory);

		victoryLabel.gameObject.SetActive (victory);
		victorySubtitle.gameObject.SetActive (victory);
	}

	public void DisableCloseButton ()
	{
		closeButton.collider.enabled = false;
	}
}
