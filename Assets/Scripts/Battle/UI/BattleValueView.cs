using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Battle;

public class BattleValueView : MonoBehaviour
{
	public bool showBattleValue = true;

	private const float NORMAL_SCALE = 1.5f;
	private const float CRITICAL_SCALE = 4f;

	private Vector3 BUFF_POSITION = new Vector3(0, 10f, 0);

	private Camera _mainCamera;

	private GameElementManager _elementManager;

	private TestGameSceneInitialization _initializer;

	void Awake ()
	{
		_elementManager = BattleGlobal.elementManager;
		_initializer = BattleGlobal.initializer;

		_mainCamera = Camera.main;
	}

	public void DisplayHPLabel(BattleAgent agent, int hpValue, bool isCritical = false, bool isMagic = false)
	{
//		if (!_initializer.showHPLabel)
//		{
//			return;
//		}
//
//        Vector3 worldPosition = MathUtil.ParseToVector3(agent.Position);
//        Vector3 screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);
//
//		BattleLabelController labelController = _elementManager.GetElement(ElementType.BattleLabel, "HPLabel").GetComponent<BattleLabelController>();
//
//		UILabel hpLabel = labelController.uiLabel;
//		hpLabel.transform.localScale = Vector3.one;
//		hpLabel.transform.localPosition = screenPosition - Vector3.right * Screen.width / 2 - Vector3.up * Screen.height / 2;
//
//		hpLabel.text = (hpValue > 0 ? "+" : "-") + Mathf.Abs(hpValue).ToString();
//
//		if (isMagic)
//		{
//			hpLabel.color = Color.blue;
//		}
//		else
//		{
//            if (agent.UnitController.Faction == Faction.Opponent)
//				hpLabel.color =  Color.white;
//			else
//				hpLabel.color =  Color.red;
//		}
//
//		TweenScale tweener = labelController.uiTweener as TweenScale;
//		tweener.tweenFactor = 0f;
//		tweener.to = Vector3.one * (isCritical ? CRITICAL_SCALE : NORMAL_SCALE);
//
//		tweener.enabled = true;
	}

    public void DisplayBuffLabel(BattleAgent agent, SkillMeta skillMeta)
	{
//		if (!_initializer.showMagicLabel)
//		{
//			return;
//		}
//
//        Vector3 worldPosition = MathUtil.ParseToVector3(agent.Position);
//        Vector3 screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);
//
//		BattleLabelController labelController = _elementManager.GetElement(ElementType.BattleLabel, "BuffLabel").GetComponent<BattleLabelController>();
//
//		UILabel buffLabel = labelController.uiLabel;
//		buffLabel.transform.localScale = Vector3.one;
//		buffLabel.transform.localPosition = screenPosition - Vector3.right * Screen.width / 2 - Vector3.up * Screen.height / 2;
//		buffLabel.text = skillMeta.metaName;
//
//		Vector3 localPosition = buffLabel.transform.localPosition;
//
//		TweenPosition tweener = labelController.uiTweener as TweenPosition;
//		tweener.tweenFactor = 0f;
//		tweener.from = localPosition;
//
//		buffLabel.color =  Color.blue;
//		localPosition += Vector3.up * 20f;
//		tweener.from = localPosition;
//		tweener.to = localPosition + BUFF_POSITION;
//		
//		tweener.enabled = true;
	}

}

