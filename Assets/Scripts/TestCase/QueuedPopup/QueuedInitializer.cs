using UnityEngine;
using System.Collections;

public class QueuedInitializer : MonoBehaviour
{
	void Awake ()
	{
		ApplicationFacade facade = ApplicationFacade.Instance;

//		facade.RegisterMediator(new SmithyMediator());
//		facade.RegisterMediator(new GoldMineSystemMediator());
//		facade.RegisterMediator(new MainCityDefenseMediator());
//		facade.RegisterMediator(new ParliamentMediator());

//		Debug.Log(3 & PanelMode.SHOW);
	}

//	void Start ()
//	{
//		ApplicationFacade facade = ApplicationFacade.Instance;
//
//		facade.SendNotification(NotificationConst.MAIN_CITY_INIT_PANEL, CityBuildingType.Smithy);
//		facade.SendNotification(NotificationConst.MAIN_CITY_INIT_PANEL, CityBuildingType.Goldmine);
//		facade.SendNotification(NotificationConst.MAIN_CITY_INIT_PANEL, CityBuildingType.Defense);
//		facade.SendNotification(NotificationConst.MAIN_CITY_INIT_PANEL, CityBuildingType.Parliament);
//	}
}

