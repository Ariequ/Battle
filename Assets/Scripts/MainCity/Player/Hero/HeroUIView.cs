using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeroUIView : BasePopupView {

	public Button closeButton;
	public Button listButton;
	public List<GameObject> heroCardList = new List<GameObject>();
	public const string CARD_PATH = "UI/MainCity/HeroUI/HeroCard";

	void Awake ()
	{
		Debug.Log(transform);
	}

	public void InitUI(HeroVO[] heroVO)
	{
		foreach (HeroVO hvo in heroVO) 
		{
			CreateHeroCard(hvo);
		}
		RectTransform grid = this.transform.Find ("ScrollView/Grid").GetComponent<RectTransform>();
		
		grid.sizeDelta = new Vector2 ((float)678 * heroVO.Length,80);
		grid.localPosition += new Vector3 ((float)678 * heroVO.Length / 2, 0, 0);
		
		grid.sizeDelta = new Vector2 (678f * heroVO.Length,0);
	}
	
	public void UpdateUI(HeroVO[] heroVO)
	{

	

	}

	public void CreateHeroCard(HeroVO heroVO)
	{
		GameObject card = ResourceFacade.Instance.LoadPrefab(PrefabType.UI, CARD_PATH);
		card.transform.parent = this.transform.Find ("ScrollView/Grid").transform;
		card.transform.localScale = Vector3.one;
		HeroCard herocard = card.transform.GetComponent<HeroCard> ();
		herocard.initHeroCard (heroVO);
		heroCardList.Add (card);
	}
	
}