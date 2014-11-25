using UnityEngine;
using System.Collections;
using Battle;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour
{
    private Camera battleCamera;
    private BattleAgentManager _battleAgentManger;
    private TroopAgent selectedTroop;
    private TroopAgent targetTroop;
    // Use this for initialization
    void Start()
    {
        battleCamera = Camera.main;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedTroop = troopAgentUnderScreenPoint(Input.mousePosition, Faction.Self, 3);
        }

        if (Input.GetMouseButton(0))
        {
            if (selectedTroop != null)
            {
                targetTroop = troopAgentUnderScreenPoint(Input.mousePosition, Faction.Opponent, 3);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedTroop != null && targetTroop != null)
            {
                Debug.Log("my troop name: " + selectedTroop.Name + " =====  target troop name: " + targetTroop.Name);
                selectedTroop.setAttackTroop(targetTroop);
            }
            selectedTroop = null;
        }
    }

    private TroopAgent troopAgentUnderScreenPoint(Vector3 screenPoint, Faction faction, float radius)
    {
        Ray ray = battleCamera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            List<BattleAgent> agents = battleAgentManger().SearchAroundPoint(MathUtil.ParseToVector2(hit.point), radius, faction);

            if (agents.Count > 0)
            {
                return battleAgentManger().GetTroopByName(agents [0].TroopName);
            }
        }

        return null;
    }

    private BattleAgentManager battleAgentManger()
    {
        if (_battleAgentManger == null)
        {
            _battleAgentManger = GetComponent<GameSceneManager>().BattleAgentManager;
        }

        return _battleAgentManger;
    }
}
