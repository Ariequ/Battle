using System;

using PureMVC.Interfaces;

public struct MainCityTipDTO
{
    public IMediator buildingMediator;

    public MainCityBuildingState buildingState;
    public CityBuildingType buildingType;
   
    public int buildingMetaID;
}

