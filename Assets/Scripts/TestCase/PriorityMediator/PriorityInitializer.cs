using UnityEngine;
using System;

public class PriorityInitializer : MonoBehaviour
{
	void Awake ()
	{
		ApplicationFacade facade = ApplicationFacade.Instance;

		facade.RegisterMediator(new PriorityMediatorX());
		facade.RegisterMediator(new PriorityMediatorA());
		facade.RegisterMediator(new PriorityMediatorB());

	}

	void Start()
	{
		ApplicationFacade.Instance.SendNotification("Start");
	}
}

