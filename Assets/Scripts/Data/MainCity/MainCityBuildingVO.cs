using System;

public enum MainCityBuildingState
{
    Empty,
    IsBuilding,
    IsUpgrading,
    Available,
}

public class MainCityBuildingVO
{
    public CityBuildingMeta meta;

    public MainCityBuildingState state;

}

