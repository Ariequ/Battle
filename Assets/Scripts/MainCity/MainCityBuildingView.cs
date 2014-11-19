using UnityEngine;
using System.Collections;
using PureMVC.Patterns;

public class MainCityBuildingView : MonoBehaviour
{
    public int buildingMetaID;

    private MainCityBuildingVO buildingVO;

    void Start ()
    {
        buildingVO = new MainCityBuildingVO();
        buildingVO.meta = MetaManager.Instance.GetCityBuildingMeta(buildingMetaID);
        buildingVO.state = MainCityBuildingState.Available;
    }
	
    public CityBuildingType BuildingType
    {
        get
        {
            return buildingVO.meta.type;
        }
    }

    public MainCityBuildingState BuildingState
    {
        get
        {
            return buildingVO.state;
        }
    }
}

