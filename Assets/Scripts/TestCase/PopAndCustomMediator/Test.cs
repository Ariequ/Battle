using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC;

public class Test : MonoBehaviour
{
    void Start()
    {
   
		Facade.Instance.RegisterMediator(new MessageRouteMediator());

//        Facade.Instance.SendNotification("testNotfication");

//        IPopupView a = PopManager.Instance().CreateAndAddPopup("Prefabs/UI/BasePanelView",true);
//		PopManager.Instance().removePopAndCleanUp(a, true);
    }
}
