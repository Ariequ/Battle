using UnityEngine;
using System.Collections;

public class UISkillButtonLayout : MonoBehaviour {

	public float marginBottom = 10f;

	void Start () 
	{
		transform.localPosition = new Vector3(0, (100 - Screen.height) / 2 + marginBottom, 0);
	}
}
