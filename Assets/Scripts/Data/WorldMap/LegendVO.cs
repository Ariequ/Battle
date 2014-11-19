using UnityEngine;

using System;
using System.Collections.Generic;

public class LegendVO
{
	public int id;

	public Vector3 lordPosition = Vector3.zero;
	public Quaternion lordRotation = Quaternion.identity;

    public int inBattleMonsterID;
    public List<int> defeatedMonsterIDList;
    public List<int> defeatedTreasureIDList;

	public MapGlobalVO globalVO;
	public List<MapBlockVO> blockVOList;
	public Dictionary<MapElementType, List<MapElementVO>> elementVOListDic;

	public Dictionary<int, string[]> blockModelDic;

	public CullingTable blockCullingTable;
	public CullingTable elementCullingTable;

	public LegendVO ()
	{
		elementVOListDic = new Dictionary<MapElementType, List<MapElementVO>>();

		blockCullingTable = new CullingTable(LegendConst.CULLING_CELL_SIZE, LegendConst.CULLING_CELL_COUNT, LegendConst.CULLING_CELL_COUNT);
		elementCullingTable = new CullingTable(LegendConst.CULLING_CELL_SIZE, LegendConst.CULLING_CELL_COUNT, LegendConst.CULLING_CELL_COUNT);
	}
}

