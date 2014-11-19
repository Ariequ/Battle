using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BattlePreparationView : BasePopupView
{
	#region public UI interface

	public UIButton closeButton;
	public UIButton detectButton;
	public UIButton startButton;

	public UITexture[] heroIcons;
	public UILabel[] heroNames;

	public UITexture[] randomRewards;
	public UILabel[] randomRewardNames;

	public UILabel woodReward;
	public UILabel oreReward;
	public UILabel coinReward;
	public UILabel expReward;
	
	public BattleFieldWindow sceneWindow;

	public UIWidget heroArea;
	public UIWidget soldierArea;

	#endregion

	public override void onEnter ()
	{
		base.onEnter ();

		sceneWindow.Display(true);
	}

	public override void onExit ()
	{
		base.onExit ();

		sceneWindow.Display(false);
	}

	public void UpdateUI(HeroVO[] heroVOArr, LevelVO levelVO)
	{
		for (int i = 0; i < heroIcons.Length; ++i)
		{
			HeroVO heroVO = heroVOArr[i];
			GameObject labelArea = heroNames[i].transform.parent.gameObject;
			string id = "default";

			if (heroVO != null)
			{
				id = heroVO.Meta.id.ToString();

				labelArea.SetActive(true);
				heroNames[i].text = heroVO.Meta.metaName;
			}
			else
			{
				labelArea.SetActive(false);
				heroNames[i].text = "";
			}

			heroIcons[i].mainTexture = Resources.Load<Texture>("UITextures/Heros/hero_" + id);
		}
	}

    public void UpdateSoldiers()
    {
    }
}
