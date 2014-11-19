using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleEmbattleView : BasePopupView {

	private const int MAX_SOLDIER_ANCHOR_USED = 5;

	public GameObject soldierPrefab;

	public UIButton confirmButton;

	public UIButton autoButton;

	public Transform[] anchors;

	public Transform[] locksAfter3;

	public UIGrid uiGrid;

	private UnitsListLayout allSoldierLayout;

	public override void onInitialize()
	{
		base.onInitialize ();

		this.allSoldierLayout = uiGrid.GetComponent<UnitsListLayout>();
 	}

	public override void onEnter()
	{
		base.onEnter ();

		UIEventListener.Get(autoButton.gameObject).onClick = OnAutoButtonClick;
	}

	public override void onExit()
	{
		base.onExit ();

		uiGrid.animateSmoothly = false;

		UIEventListener.Get(autoButton.gameObject).onClick = null;
	}

	public void UpdateUI (SoldierVO[] allSoldiers, SoldierVO[] battleSoldiers, int unlockedAnchorCount)
	{
		ClearUI();

		Dictionary<int, SoldierVO> onBattleFields = new Dictionary<int, SoldierVO>();

		for (int i = 0; i < anchors.Length && i < battleSoldiers.Length; i++)
		{
			SoldierVO soldierVO = battleSoldiers[i];

			if (soldierVO != null)
			{
				SoldierMeta soldierMeta = soldierVO.Meta;
				
				Transform instance = InstantiateSoldierUnit(soldierPrefab, soldierVO, anchors[i]);

				UnitsAnchorLayout anchorLayout = anchors[i].GetComponent<UnitsAnchorLayout>();
				anchorLayout.Layout(instance);
				
				onBattleFields.Add(soldierMeta.id, soldierVO);
			}
		}

		foreach (SoldierVO soldierVO in allSoldiers)
		{
			SoldierMeta soldierMeta = soldierVO.Meta;

			if (!onBattleFields.ContainsKey(soldierMeta.id))
			{
				Transform instance = InstantiateSoldierUnit(soldierPrefab, soldierVO, allSoldierLayout.transform);

				Vector3 position = instance.localPosition;
				position.z = 0;
				instance.localPosition = position;

				allSoldierLayout.Layout(instance);
			}
		}

		for (int i = 0; i < Math.Min(locksAfter3.Length, unlockedAnchorCount); i++) 
		{
			Transform lockableAnchor = locksAfter3[i];
			lockableAnchor.gameObject.SetActive(false);
			lockableAnchor.parent.collider.enabled = true;
		}

		StartCoroutine(EnableAnimateSmoothly());
	}

	public SoldierVO[] GetBattleSoldiers ()
	{
		SoldierVO[] battleSoldiers = new SoldierVO[MAX_SOLDIER_ANCHOR_USED];

		for (int i = 0; i < battleSoldiers.Length; i++)
		{
			SoldierUnitInfo unitInfo = anchors[i].GetComponentInChildren<SoldierUnitInfo>();
			battleSoldiers[i] = unitInfo != null ? unitInfo.soldierVO : null;
		}

		return battleSoldiers;
	}

	private void ClearUI ()
	{
		foreach (Transform anchor in anchors)
		{
			AbstractAnchorInfo child = anchor.GetComponentInChildren<AbstractAnchorInfo>();

			if (child != null)
			{
				Transform childTransform = child.transform;
				childTransform.parent = null;
				Destroy(childTransform.gameObject);
			}
		}
		
		AbstractAnchorInfo[] children = allSoldierLayout.GetComponentsInChildren<AbstractAnchorInfo>();

		for (int i = 0; i < children.Length; i++)
		{
			Transform childTransform = children[i].transform;
			childTransform.parent = null;
			Destroy(childTransform.gameObject);
		}

		foreach (Transform lockableAnchor in this.locksAfter3) 
		{
			lockableAnchor.gameObject.SetActive(true);
			lockableAnchor.parent.collider.enabled = false;
		}
	}

	private void OnAutoButtonClick (GameObject button)
	{
		int readAnchorCount = 0;
		int enabledAnchorCount = 0;

		IList<Transform> unbindedAnchors = new List<Transform>();

		foreach (Transform anchor in anchors)
		{
			if (anchor.collider.enabled)
			{
				enabledAnchorCount ++;

				if (null == anchor.GetComponentInChildren<AbstractAnchorInfo>())
				{
					unbindedAnchors.Add(anchor);
				}
				else
				{
					readAnchorCount ++;
				}
			}
		}

		IList<SoldierUnitInfo> maxLevelSoldiers = new List<SoldierUnitInfo>();
		SoldierUnitInfo[] soldierUnitInfos = allSoldierLayout.GetComponentsInChildren<SoldierUnitInfo>();

		foreach (SoldierUnitInfo unitInfo in soldierUnitInfos)
		{
			bool unitInfoUsed = false;

			for (int i = 0; i < maxLevelSoldiers.Count && i < unbindedAnchors.Count; i++)
			{
				if (unitInfo.soldierVO.level > maxLevelSoldiers[i].soldierVO.level)
				{
					maxLevelSoldiers.Insert(i, unitInfo);
					unitInfoUsed = true;

					if (maxLevelSoldiers.Count > unbindedAnchors.Count)
					{
						maxLevelSoldiers.RemoveAt(maxLevelSoldiers.Count - 1);
					}

					break;
				}
			}

			if (!unitInfoUsed && maxLevelSoldiers.Count < unbindedAnchors.Count)
			{
				maxLevelSoldiers.Add(unitInfo);
			}
		}

		int unbindedAnchorCount = Math.Min (enabledAnchorCount, MAX_SOLDIER_ANCHOR_USED) - readAnchorCount;

		for (int i = 0; i < unbindedAnchorCount; i++)
		{
			if (i < maxLevelSoldiers.Count)
			{
				Transform soldier = maxLevelSoldiers[i].transform;
				UIDragDropItem soldierDragDropItem = soldier.GetComponent<UIDragDropItem>();

				soldierDragDropItem.OnDragDropRelease(unbindedAnchors[i].gameObject);
				uiGrid.repositionNow = true;
			}
			else
			{
				break;
			}
		}
	}

	private IEnumerator EnableAnimateSmoothly ()
	{
		yield return new WaitForSeconds(1);

		uiGrid.animateSmoothly = true;
	}

	private static Transform InstantiateSoldierUnit (GameObject soldierPrefab, SoldierVO soldierVO, Transform parent)
	{
		Transform instance = ((GameObject) Instantiate(soldierPrefab)).transform;
		instance.GetComponent<SoldierUnitInfo>().soldierVO = soldierVO;

		SoldierRenderer soldierRenderer = instance.GetComponent<SoldierRenderer> ();
		soldierRenderer.name.text = soldierVO.Meta.metaName;
		soldierRenderer.texture.mainTexture = Resources.Load<Texture>("UITextures/Soldiers/soldier_" + soldierVO.Meta.id.ToString());

		instance.parent = parent;
		instance.localScale = Vector3.one;

		return instance;
	}
}
