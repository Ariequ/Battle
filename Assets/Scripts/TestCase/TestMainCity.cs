using UnityEngine;
using System.Collections;

public class TestMainCity : MonoBehaviour 
{
    void Awake ()
    { 
        ApplicationFacade facade = ApplicationFacade.Instance;
        
        facade.RegisterMediator(new WarTempleMediator());
        facade.RegisterMediator(new TransportPointDetailMediator());
    }
    
    void Start ()
    {
//        ApplicationFacade.Instance.SendNotification(NotificationConst.WARTEMPLE_SHOW);
    }
}
