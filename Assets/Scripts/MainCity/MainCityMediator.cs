using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public class MainCityMediator : Mediator
{
    new public static String NAME = "MainCityMediator";

	private const string VIEW_NAME = "MainCityUI";

    private Dictionary<CityBuildingType, IMediator> buildingMediatorDic = new Dictionary<CityBuildingType, IMediator>()
    {
        {CityBuildingType.Parliament, new ParliamentMediator()},
        {CityBuildingType.Defense, new MainCityDefenseMediator()},
        {CityBuildingType.Goldmine, new GoldMineSystemMediator()},
        {CityBuildingType.Smithy, new SmithyMediator()}
    };

    private MainCityView mainCityView;
//    private ZoomCameraController zoomController;

    private MainCityBuildingView[] buildingViewArray;

    public MainCityMediator() : base (NAME)
    {
        GameObject mainCityGO = GameObject.Find(VIEW_NAME);
        mainCityView = mainCityGO.GetComponent<MainCityView>();
//        zoomController = mainCityGO.GetComponentInChildren<ZoomCameraController>();

        buildingViewArray = mainCityView.GetComponentsInChildren<MainCityBuildingView>();
        foreach (MainCityBuildingView view in buildingViewArray)
        {
            UIEventListener.Get(view.gameObject).onClick = onBuildingViewClick;
        }

        MainCityTipMediator tipMediator = new MainCityTipMediator();
        ApplicationFacade.Instance.RegisterMediator(tipMediator);
    }

    public override void OnRemove()
    {
        ApplicationFacade facade = ApplicationFacade.Instance;
        facade.RemoveMediator(MainCityTipMediator.NAME);

        foreach (IMediator mediator in buildingMediatorDic.Values)
        {
            if (facade.HasMediator(mediator.MediatorName))
                facade.RemoveMediator(mediator.MediatorName);
        }
    }
    
    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();
        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
    }

    private void handleTipHide()
    {
//        zoomController.SetFocusPosition(Vector3.zero);
    }

    private void onBuildingViewClick(GameObject go)
    {
        MainCityBuildingView view = go.GetComponent<MainCityBuildingView>();
        IMediator mediator = buildingMediatorDic[view.BuildingType];

        MainCityTipDTO dto = new MainCityTipDTO();
        dto.buildingMediator = mediator;
        dto.buildingState = view.BuildingState;
        dto.buildingType = view.BuildingType;
        dto.buildingMetaID = view.buildingMetaID;

//        zoomController.SetFocusPosition(go.transform.localPosition);

        SendNotification(NotificationConst.MAIN_CITY_TIP_SHOW, dto);

    }


}

