using System.Collections;
using UnityEngine;

public class LegendShadowController : MonoBehaviour
{
	public float updateInterval = 0.2f;

	private float prevTime;

	private Transform tran;

    void Awake ()
    {
		prevTime = Time.time;

		tran = transform;
    }

	public void UpdateRotation (Vector3 velocity)
	{
		if (Time.time - prevTime <= updateInterval) return;

		Vector3 eulerAngles = new Vector3(90f, 0, 0);
		if (velocity.y != 0)
		{
			float upAngle = Mathf.Atan2(velocity.y, Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z)) * 180 / Mathf.PI;
			eulerAngles = new Vector3(90f, upAngle, 0);
		}
		tran.localEulerAngles = eulerAngles;
	}
}

