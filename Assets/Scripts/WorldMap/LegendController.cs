using UnityEngine;
using System.Collections;

public class LegendController : MonoBehaviour 
{
    void Awake () 
    {
        ApplicationFacade.Instance.RegisterMediator (new LegendMediator ());

        ApplicationFacade.Instance.RegisterMediator (new GoldMineMediator ());
        ApplicationFacade.Instance.RegisterMediator (new WarTempleMediator ());
        ApplicationFacade.Instance.RegisterMediator (new WisdomTreeMediator ());
        ApplicationFacade.Instance.RegisterMediator (new LuckyGoddessMediator ());
        ApplicationFacade.Instance.RegisterMediator (new MecaneryBaseMediator ());
        ApplicationFacade.Instance.RegisterMediator (new PrisonMediator ());

        ApplicationFacade.Instance.RegisterMediator (new BattlePreparationMediator (PopupManager.Instance));
        ApplicationFacade.Instance.RegisterMediator (new BattleEmbattleMediator ());
        ApplicationFacade.Instance.RegisterMediator (new BattleOpponentMediator ());
        ApplicationFacade.Instance.RegisterMediator (new ChooseHeroMediator ());
        ApplicationFacade.Instance.RegisterMediator(new BattleFieldMediator());
    }

    void OnDestroy ()
    {
        ApplicationFacade.Instance.RemoveMediator (LegendMediator.NAME);

        ApplicationFacade.Instance.RemoveMediator (GoldMineMediator.NAME);
        ApplicationFacade.Instance.RemoveMediator (WarTempleMediator.NAME);
        ApplicationFacade.Instance.RemoveMediator (WisdomTreeMediator.NAME);
        ApplicationFacade.Instance.RemoveMediator (LuckyGoddessMediator.NAME);
        ApplicationFacade.Instance.RemoveMediator (MecaneryBaseMediator.NAME);
        ApplicationFacade.Instance.RemoveMediator (PrisonMediator.NAME);

        ApplicationFacade.Instance.RemoveMediator (BattlePreparationMediator.NAME);
        ApplicationFacade.Instance.RemoveMediator (BattleEmbattleMediator.NAME);
        ApplicationFacade.Instance.RemoveMediator (BattleOpponentMediator.NAME);
        ApplicationFacade.Instance.RemoveMediator (ChooseHeroMediator.NAME);
        ApplicationFacade.Instance.RemoveMediator (BattleFieldMediator.NAME);

    }

    public void BackToMainInterface ()
    {
        GameGlobal.LoadSceneByName (SceneNameConst.MAIN_INTERFACE);
    }

    
    public void OnTransportPointPressed()
    {
        if (!ApplicationFacade.Instance.HasMediator(TransportPointMediator.NAME))
        {
            ApplicationFacade.Instance.RegisterMediator(new TransportPointMediator());
        }

        ApplicationFacade.Instance.SendNotification(NotificationConst.TRANSPORTPOINT_SHOW);
    }
}
