using UnityEngine;
using System.Collections;

public class CameraMotionController : MonoBehaviour {

	public float speed = 20f;
	
	void Update () 
	{
		float vz = Input.GetAxis ("Horizontal") * Time.deltaTime * speed;
		float vx = -Input.GetAxis ("Vertical") * Time.deltaTime * speed;

		Vector3 translateValue = new Vector3 (vx, 0, vz);

		if (Input.GetKey(KeyCode.O))
		{
			translateValue.y -= 0.15f;
		}
		else if (Input.GetKey(KeyCode.P))
		{
			translateValue.y += 0.15f;
		}

		transform.Translate(translateValue, Space.World);
	}
}
