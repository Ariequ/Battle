using UnityEngine;
using System.Collections;

using Battle;

public class CastSkillTouchHandler : AbstractTouchHandler
{
	public GameObject skillMarker;

	private Vector3 skillPosition;

	private float _radius;

	private SkillMeta _currentSkillMeta;

	protected void Awake ()
	{
		AddTouchHandler();

		_radius = skillMarker.renderer.bounds.size.x;
	}

	public override void OnPress()
	{

	}

	public override void OnStay()
	{
		Debug.Log("on stay");

		skillMarker.SetActive(true);

		Vector3 mousePosition = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay (mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast(ray, out hitInfo, 9999, 1 << Layers.DEFAULT))
		{
			skillPosition = hitInfo.point;
			skillPosition.Set(skillPosition.x + ray.direction.x * _radius, 0.1f, skillPosition.z + ray.direction.z * _radius);

			skillMarker.transform.position = skillPosition;
		}
	}

	public override void OnRelease()
	{
		BattleGlobal.gameController.GetComponent<GameSceneManager>().ExecuteSkill(this._currentSkillMeta, skillPosition);

		this._currentSkillMeta = null;

		skillMarker.SetActive(false);
		isActive = false;
	}

	public void StartDraggingMagic(int skillID)
	{
		this._currentSkillMeta = MetaManager.Instance.GetSkillMeta(skillID);

		float radius = _currentSkillMeta.edgeRadius;
		skillMarker.transform.localScale = Vector3.one * radius / 5f;

		isActive = true;
	}
}

