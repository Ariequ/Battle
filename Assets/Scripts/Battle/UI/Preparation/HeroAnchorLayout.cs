using UnityEngine;
using System.Collections;

public class HeroAnchorLayout : UIDragDropLayout 
{
	public Transform sendBackTarget;

	[HideInInspector]
	public GameObject heroPrefab;
	
	private UIDragDropLayout sendBackLayout;

	private HeroAnchorInfo currentAnchorInfo;

	private UISprite anchorSprite;

	private UITexture anchorTexture;
	
	void Start () 
	{
		if (this.sendBackTarget != null)
		{
			this.sendBackLayout = sendBackTarget.GetComponent<UIDragDropLayout>();
		}

		this.currentAnchorInfo = GetComponent<HeroAnchorInfo>();
		this.anchorSprite = GetComponent<UISprite>();
		this.anchorTexture = GetComponentInChildren<UITexture>();
	}
	
	override public void Layout (Transform item)
	{
		HeroAnchorInfo heroInfo = item.GetComponent<HeroAnchorInfo> ();
		HeroMeta heroMeta = heroInfo.heroVO.Meta;
		
		if (currentAnchorInfo.heroVO == null || ! ReferenceEquals(currentAnchorInfo.heroVO.Meta, heroMeta))
		{
			item.parent = null;
			Destroy(item.gameObject);

			if (currentAnchorInfo.heroVO != null && sendBackTarget != null)
			{
				Transform currentUnit = InstantiateHeroUnit(heroPrefab, currentAnchorInfo.heroVO, sendBackTarget);
				
				if (this.sendBackLayout != null)
				{
					sendBackLayout.Layout(currentUnit);
				}
			}

			currentAnchorInfo.heroVO = heroInfo.heroVO;
			
			anchorSprite.spriteName = HeroQuality.Purple1 == heroMeta.quality ? "hero_card_shading_purple" : "hero_card_shading_green";
			anchorTexture.mainTexture = Resources.Load<Texture>("UITextures/Heros/card_hero_" + heroMeta.id.ToString());
		}
	}

	public static Transform InstantiateHeroUnit (GameObject heroPrefab, HeroVO heroVO, Transform parent)
	{

		Transform instance = ((GameObject) Instantiate(heroPrefab)).transform;
		instance.GetComponent<HeroAnchorInfo>().heroVO = heroVO;
		
		HeroRenderer heroRenderer = instance.GetComponent<HeroRenderer>();
		heroRenderer.texture.mainTexture = Resources.Load<Texture>("UITextures/Heros/hero_" + heroVO.Meta.id.ToString());
		heroRenderer.name.text = heroVO.Meta.metaName;

		instance.parent = parent;
		instance.localScale = Vector3.one;
		instance.localPosition = new Vector3 (0, 0, 0);
		
		return instance;
	}
}
