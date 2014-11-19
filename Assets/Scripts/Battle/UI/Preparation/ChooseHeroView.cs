using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChooseHeroView : BasePopupView
{
	private const float ITEM_WIDTH = 150f;

	#region public UI interface

	public GameObject heroIconPrefab;

	public UIButton autoButton;

	public UIButton recruitButton;

	public UIButton returnButton;

	public UISprite[] heroAnchors;

	public UIGrid scrollGrid;

	#endregion

	private UnitsListLayout allHeroLayout;
	
	public override void onInitialize()
	{
		base.onInitialize();

		this.allHeroLayout = scrollGrid.GetComponent<UnitsListLayout>();

		HeroAnchorLayout[] anchorLayouts = GetComponentsInChildren<HeroAnchorLayout> ();

		foreach (HeroAnchorLayout anchorLayout in anchorLayouts)
		{
			anchorLayout.heroPrefab = this.heroIconPrefab;
		}
	}

	public override void onEnter()
	{
		base.onEnter ();

		UIEventListener.Get(autoButton.gameObject).onClick = OnAutoButtonClick;
	}

	public override void onExit()
	{
		base.onExit ();

		UIEventListener.Get(autoButton.gameObject).onClick = null;
	}

	public HeroVO[] GetBattleHeroes ()
	{
		HeroVO[] result = new HeroVO[heroAnchors.Length];

		for (int i = 0; i < heroAnchors.Length; i++)
		{
			result[i] = heroAnchors[i].GetComponent<HeroAnchorInfo>().heroVO;
		}

		return result;
	}

	public void UpdateUI (HeroVO[] allHeroes, HeroVO[] battleHeroes)
	{
		ClearUI();
		
		Dictionary<int, HeroVO> onBattleFields = new Dictionary<int, HeroVO>();
		
		for (int i = 0; i < heroAnchors.Length && i < battleHeroes.Length; i++)
		{
			HeroVO heroVO = battleHeroes[i];
			
			if (heroVO != null)
			{
				HeroMeta heroMeta = heroVO.Meta;
				UISprite heroAnchor = heroAnchors[i];

				heroAnchor.GetComponent<HeroAnchorInfo>().heroVO = heroVO;
				heroAnchor.spriteName = HeroQuality.Purple1 == heroMeta.quality ? "hero_card_shading_purple" : "hero_card_shading_green";
				heroAnchor.GetComponentInChildren<UITexture>().mainTexture = Resources.Load<Texture>("UITextures/Heros/card_hero_" + heroVO.Meta.id.ToString());
				
				onBattleFields.Add(heroMeta.id, heroVO);
			}
		}
		
		foreach (HeroVO heroVO in allHeroes)
		{
			HeroMeta heroMeta = heroVO.Meta;
			
			if (!onBattleFields.ContainsKey(heroMeta.id))
			{
				Transform instance = HeroAnchorLayout.InstantiateHeroUnit(heroIconPrefab, heroVO, allHeroLayout.transform);
				
				Vector3 position = instance.localPosition;
				position.z = 0;
				instance.localPosition = position;
				
				allHeroLayout.Layout(instance);
			}
		}
		
		StartCoroutine(EnableAnimateSmoothly());
	}

	private void ClearUI ()
	{
		foreach (UISprite anchor in heroAnchors)
		{
			anchor.spriteName = "hero_card_shading_default";
			anchor.GetComponent<HeroAnchorInfo>().heroVO = null;
			anchor.GetComponentInChildren<UITexture>().mainTexture = null;
		}
		
		AbstractAnchorInfo[] children = allHeroLayout.GetComponentsInChildren<AbstractAnchorInfo>();
		
		for (int i = 0; i < children.Length; i++)
		{
			Transform childTransform = children[i].transform;
			childTransform.parent = null;
			Destroy(childTransform.gameObject);
		}
	}

	private IEnumerator EnableAnimateSmoothly ()
	{
		yield return new WaitForSeconds(1);
		
		scrollGrid.animateSmoothly = true;
	}

	private void OnAutoButtonClick (GameObject button)
	{
		foreach (UISprite anchor in heroAnchors)
		{
			HeroAnchorInfo heroAnchorInfo = anchor.GetComponentInChildren<HeroAnchorInfo>();

			if (heroAnchorInfo.heroVO != null)
			{
				HeroAnchorLayout.InstantiateHeroUnit(heroIconPrefab, heroAnchorInfo.heroVO, scrollGrid.transform);
				heroAnchorInfo.heroVO = null;
			}	                             
		}
		
		IList<HeroAnchorInfo> maxLevelHeros = new List<HeroAnchorInfo>();
		HeroAnchorInfo[] heroAnchorInfos = allHeroLayout.GetComponentsInChildren<HeroAnchorInfo>();
		
		foreach (HeroAnchorInfo anchorInfo in heroAnchorInfos)
		{
			bool anchorInfoUsed = false;
			
			for (int i = 0; i < maxLevelHeros.Count && i < heroAnchors.Length; i++)
			{
				if (anchorInfo.heroVO.level > maxLevelHeros[i].heroVO.level)
				{
					maxLevelHeros.Insert(i, anchorInfo);
					anchorInfoUsed = true;
					
					if (maxLevelHeros.Count > heroAnchors.Length)
					{
						maxLevelHeros.RemoveAt(maxLevelHeros.Count - 1);
					}
					
					break;
				}
			}
			
			if (!anchorInfoUsed && maxLevelHeros.Count < heroAnchors.Length)
			{
				maxLevelHeros.Add(anchorInfo);
			}
		}
		
		for (int i = 0; i < heroAnchors.Length; i++)
		{
			if (i < maxLevelHeros.Count)
			{
				Transform hero = maxLevelHeros[i].transform;
				UIDragDropItem heroDragDropItem = hero.GetComponent<UIDragDropItem>();
				
				heroDragDropItem.OnDragDropRelease(heroAnchors[i].gameObject);
				scrollGrid.repositionNow = true;
			}
			else
			{
				break;
			}
		}

//		Debug.Log (scrollGrid.GetComponentsInChildren<UIDragDropItem>().Length);
	}
}

