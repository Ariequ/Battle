using UnityEngine;
using System.Collections;
using System.IO;

public class BattleFieldView : MonoBehaviour
{
    private const string SOLDIER_ANIM_SUFFIX = "_Anim";

	public Transform centerAnchor;

    public Transform soldiersContainer;

    private Vector3[] troopPositions;
    private SoldierVO[] soldierVOs;
	
	public void UpdateBattleField(Faction faction, SoldierVO[] soldierVOArray, LevelVO levelVO)
	{
        while (soldiersContainer.childCount > 0)
        {
            Transform child = soldiersContainer.GetChild(0);
            child.parent = null;
            Destroy(child.gameObject);
        }
		
		soldierVOs = soldierVOArray;
        troopPositions = new Vector3[soldierVOs.Length];
		
		for (int i = 0; i < soldierVOs.Length; ++i)
		{
            Vector3 troopPosition = MathUtil.ParseToVector3(faction == Faction.Self ? levelVO.Meta.selfPositions[i] : levelVO.Meta.opponentPositions[i]);
            troopPositions[i] = centerAnchor.position + troopPosition;

			SetTroopGameObject(soldierVOs[i], i);
		}

	}
	
	private void SetTroopGameObject(SoldierVO soldierVO, int index)
	{
        Vector3 troopPosition = troopPositions[index];
        
        if (soldierVO != null)
        {
            SoldierMeta meta = soldierVO.Meta;
            
            int soldierCount = meta.maxUnitCount;
            Battle.Vector2[] anchorPositions = AnchorConfigManager.Singleton.GetSoldierAnchors(meta.sizeLevel);
			
			for (int i = 0; i < soldierCount; ++i)
			{
                GameObject soldier = ResourceFacade.Instance.LoadPrefab(PrefabType.Soldier, meta.prefabPath + SOLDIER_ANIM_SUFFIX);
                Vector3 soldierPosition = new Vector3(anchorPositions[i].x(), 0f, anchorPositions[i].y());
                soldier.transform.position = troopPosition + soldierPosition;
                soldier.transform.parent = soldiersContainer;
			}
		}
	}
}

