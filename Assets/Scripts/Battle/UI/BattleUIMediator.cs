using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;
using PureMVC.Interfaces;

using Battle;

public class BattleUIMediator : Mediator, IMessageDriven
{
    new public static String NAME = "BattleUIMediator";

	private BattleUIView battleUIView;

    private BattleValueProxy selfValueProxy;

    public BattleUIMediator() : base (NAME)
    {
        selfValueProxy = ApplicationFacade.Instance.RetrieveValueProxy(Faction.Self);
    }

    public override IList<string> ListNotificationInterests()
    {
        List<string> notifications = new List<string>();

        notifications.Add(NotificationConst.SHOW_BATTLE_UI);

        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name)
        {
        case NotificationConst.SHOW_BATTLE_UI:
			showMagicUI(notification.Body as BattleUIView);
            break;
        }
    }

	public override void OnRemove()
	{
		foreach (SkillButtonView skillButtonView in battleUIView.skillButtonList)
		{
			if (skillButtonView != null)
			{
				UIEventListener.Get(skillButtonView.gameObject).onPress -= onSkillButtonPress;
			}
		}
	}

    public String[] GetMessageTypes ()
    {
        String[] messageTypes = new string[]
        {
            Messages.UPDATE_BATTLE_FORCE,
        };
        return messageTypes;
    }

    public void OnMessageArrived (MessageContext context)
    {
		float selfRatio = (float)context.GetConetextValue(Faction.Self.ToString());
		float opponentRatio = (float)context.GetConetextValue(Faction.Opponent.ToString());

        switch(context.messageType)
        {
        case Messages.UPDATE_BATTLE_FORCE:
			battleUIView.UpdateBattleForece(selfRatio, opponentRatio);
            break;
        }
    }

	private void showMagicUI(BattleUIView battleUIView)
    {
		this.battleUIView = battleUIView;

		battleUIView.UpdateUI(selfValueProxy.GetSkillMetaList());
		battleUIView.UpdateBattleForece(1f, 1f);

		foreach (SkillButtonView skillButtonView in battleUIView.skillButtonList)
		{
			UIEventListener.Get(skillButtonView.gameObject).onPress += onSkillButtonPress;
		}
    }

	private void onSkillButtonPress(GameObject button, bool isPressed)
    {
		if (isPressed)
        {
			SkillButtonView buttonView = button.GetComponent<SkillButtonView>();
//            SkillMeta skillMeta = MetaManager.Instance.GetSkillMeta(buttonView.SkillID);

//            if (selfValueProxy.GetAnger() >= skillMeta.angerExpense)
            {
				battleUIView.StartMagicDragging(buttonView);
            }
        }
    }
}

