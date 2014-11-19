using UnityEngine;
using System.Collections;

public class BattleOpponentView : BasePopupView
{
	public UISprite closeButton;

	public UILabel description;

	public UIButton startButton;

	public UIButton returnButton;

	public BattleFieldWindow sceneWindow;

	public UITexture[] magicIcons;

	public UILabel[] magicNames;

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

	public void UpdateUI (SkillMeta[] skillMetas, SoldierMeta[] soldiers, string levelDesc)
	{
		for (int i = 0; i < skillMetas.Length; ++i)
		{
			SkillMeta skillMeta = skillMetas[i];
			
			if (skillMeta != null)
			{
				magicIcons[i].mainTexture = Resources.Load<Texture>("UITextures/Magics/magic_" + skillMeta.id.ToString());
				magicNames[i].text = skillMeta.metaName;
				magicNames[i].transform.parent.gameObject.SetActive(true);
			}
			else
			{
				magicIcons[i].mainTexture = null;
				magicNames[i].transform.parent.gameObject.SetActive(false);
			}
		}

//		description.text = levelDesc;
	}
}

