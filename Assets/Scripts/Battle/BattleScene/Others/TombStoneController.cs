using UnityEngine;
using System.Collections;

public class TombStoneController : MonoBehaviour
{
	private static int counter = 0;

	public float timeStamp;

	public string troopTag;

	public TroopDefinition troopDefinition;

	[HideInInspector]
	public ElementController elementController;

	private int id;

	void Awake ()
	{
#if BATTLE_SIMULATOR
        return;
#endif

		elementController = GetComponent<ElementController>();

		transform.localRotation = MathUtil.LookRotationXZ(Camera.main.transform.position - transform.position);
		transform.localEulerAngles += Random.onUnitSphere * 5;

		id = counter++;
	}

	public int ID
	{
		get { return id; }
	}
}

