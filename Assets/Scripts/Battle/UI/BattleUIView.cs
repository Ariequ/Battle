using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleUIView : MonoBehaviour
{
    public UILabel selfLordNameLabel;
    public UILabel opponentLordNameLabel;

    public UIProgressBar selfBattleForceBar;
    public UIProgressBar opponentBattleForceBar;

    public MachineButtonView machineButtonView;
	public List<SkillButtonView> skillButtonList;

	public UIGrid skillButtonGrid;

	private CastSkillTouchHandler magicTouchHandler;

	void Awake ()
	{
		magicTouchHandler = GetComponent<CastSkillTouchHandler>();
		skillButtonGrid.gameObject.SetActive (false);
	}

	public void StartMagicDragging (SkillButtonView buttonView)
	{
		magicTouchHandler.StartDraggingMagic(buttonView.SkillID);
	}
	
    public void UpdateUI(List<SkillMeta> skillMetaList)
    {
		int i;

        for (i = 0; i < skillMetaList.Count; ++i)
        {
            SkillButtonView buttonView = skillButtonList[i];
            buttonView.Initialize(skillMetaList[i]);
        }

		for (; i < skillButtonList.Count; ++i)
		{
			skillButtonList[i].gameObject.SetActive(false);
		}

		skillButtonGrid.repositionNow = true;
		skillButtonGrid.gameObject.SetActive (true);
    }

	public void UpdateBattleForece (float selfRatio, float opponentRatio)
    {
		selfBattleForceBar.value = selfRatio;
		opponentBattleForceBar.value = opponentRatio;
    }

    public void UpdateAnger (int skillID, int anger)
    {
    }
}
