using UnityEngine;
using System.Collections;

public class TroopLabelDisplayer : MonoBehaviour
{
//  private TroopInfo _troopInfo;

    private BattleLabelController _labelController;
    private UnitController _leaderSoldier;
    private GameElementManager _elementManager;
    private Camera _mainCamera;

    void Awake()
    {
//      _troopInfo = GetComponent<TroopInfo>();

        _elementManager = BattleGlobal.elementManager;

        _mainCamera = Camera.main;
    }

    void Start()
    {
//        if (_leaderSoldier == null)
//        {
//          _leaderSoldier = _troopInfo.leaderUnit;
//        }
//
//        _labelController = _elementManager.GetElement(ElementType.BattleLabel, "TroopSoldierCountLabel").GetComponent<BattleLabelController>();
//
//        UILabel troopLabel = _labelController.uiLabel;
//        troopLabel.transform.localScale = Vector3.one;
//        troopLabel.color = _leaderSoldier.Faction == Faction.Self ? Color.red : Color.cyan;
//
//        StartCoroutine(UpdateLabelPosition());
    }

//    private IEnumerator UpdateLabelPosition()
//    {
//       while (true)
//       {
//            yield return new WaitForSeconds(0.03f);
//
//          if (_troopInfo.soldierCount > 0)
//          {
//              _leaderSoldier = _troopInfo.leaderUnit;
//              
//              if (_leaderSoldier != null)
//              {
//                  Vector3 screenPosition = _mainCamera.WorldToScreenPoint(_leaderSoldier.transform.position);
//                  
//                  UILabel troopLabel = _labelController.uiLabel;
//                  troopLabel.transform.localPosition = screenPosition - Vector3.right * Screen.width / 2 - Vector3.up * Screen.height / 2;
//                  troopLabel.text = _troopInfo.soldierCount.ToString();
//              }
//          }
//          else
//          {
//              _labelController.elementController.Recycle();
//
//              break;
//       }
//   }
}
