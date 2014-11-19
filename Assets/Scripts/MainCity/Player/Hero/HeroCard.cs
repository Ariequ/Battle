using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeroCard : MonoBehaviour 
{


	public int id;

	public HeroMeta heroMeta;

	public HeroVO heroVO;

	public Text name;

	public Text lv;

	public Text att;

	public Text def;

	public Text hp;

	public Text Fighting;

	public Text FightingNum;

	public Image HeroIcon;

	public Image HeroQuality;

	public const string starPic = "UITextures/CommonIcon/star";

	public const string starShadowPic = "UITextures/CommonIcon/starShadow";

	public const string heroQualityPic = "UITextures/Heros/Quality/";



	public void initHeroCard(HeroVO heroVO)
	{
		this.heroVO = heroVO;
		heroMeta = heroVO.Meta;
		id = heroVO.ID;
		name.text = Language.GetContentOfKey ("" + heroMeta.nameID);
		lv.text = heroVO.level.ToString ();
		att.text = heroVO.Attack.ToString ();
		def.text = heroVO.Defense.ToString ();
		hp.text = heroVO.HP.ToString ();
		//Fighting.text = Language.GetContentOfKey ();
		Fighting.text = "战斗力";
		FightingNum.text = heroVO.Fighting.ToString ();
		initStar (heroVO.starLevel);
		HeroQuality.color = heroMeta.color[(int)heroMeta.quality-1];
	}

	public void initStar(int num)
	{
		for (int i= 0; i<5; i++) 
		{
			Image star = this.transform.FindChild("starItem/star"+(i+1)).GetComponent<Image>();
			if(i<num)
				star.sprite= Resources.Load(starPic,typeof(Sprite)) as Sprite;
			else
				star.sprite= Resources.Load(starShadowPic,typeof(Sprite)) as Sprite;

		}
	}

	
}
