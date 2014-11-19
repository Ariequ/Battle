using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using System.Collections.Generic;

public class ChangeableListNotificationMediator : Mediator
{
	public new const string NAME = "ChangeableListNotificationMediator";

	public ChangeableListNotificationMediator()
		: base(NAME, null) { }

    public ChangeableListNotificationMediator(string mediatorName)
        : base(mediatorName, null) { }

    public override IList<string> ListNotificationInterests()
    {
        return ChangeableListNotification();
    }

    public virtual IList<string> ChangeableListNotification()
    {
        return new List<string>();
    }

    public virtual void ChangeListNotification()
    {
        Facade.RemoveMediator(m_mediatorName);
        Facade.RegisterMediator(this);
    }
}
