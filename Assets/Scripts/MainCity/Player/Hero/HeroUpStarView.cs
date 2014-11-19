using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeroUpStarView : BasePopupView {


	public Button closeButton;
	public Button sureButton;

	public HeroCard heroCard1;
	public HeroCard heroCard2;

	public Image skillIcon1; 
	public Image skillIcon2; 

	public Text skillName1;
	public Text skillName2;

	public Text skillExplain1;
	public Text skillExplain2;

	public Text useGem;
	public Text useGold;
	public Text useGemText;
	public Text useGoldText;

	public Text myGem;
	public Text myGold;
	public Text myGemText;
	public Text myGoldText;

	public Text UpStarText;
	public Text UpStarConditionText;

	public Text closeText;
	public Text sureText;

	public const string starPic = "UITextures/CommonIcon/star";
	
	public const string starShadowPic = "UITextures/CommonIcon/starShadow";

	public void InitUI(HeroVO heroVO)
	{
		useGemText.text = "消耗水晶：";
		useGoldText.text = "消耗金币：";
		myGemText.text = "当前拥有：";
		myGoldText.text = "当前拥有：";
		UpStarText.text = "升星";
		UpStarConditionText.text = "升星条件";
		closeText.text = "返回";
		sureText.text = "确定";
	}

	public void initStar(int num,string path)
	{
		for (int i= 0; i<5; i++) 
		{
			Image star = this.transform.FindChild(path+(i+1)).GetComponent<Image>();
			if(i<num)
				star.sprite= Resources.Load(starPic,typeof(Sprite)) as Sprite;
			else
				star.sprite= Resources.Load(starShadowPic,typeof(Sprite)) as Sprite;
			
		}
	}
	
}
