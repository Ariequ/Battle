using UnityEngine;
using System.Collections;

public class TouchMove : MonoBehaviour 
{
	private Quaternion rotation;

	void Start () 
	{
		rotation = Quaternion.Euler(new Vector3(0, 54, 0));
	}
	
	void LateUpdate () 
	{
		float x = Input.GetAxis("Mouse X");
		float y = Input.GetAxis("Mouse Y");
		Vector3 velocity = new Vector3(-x, 0, -y);

		velocity = rotation * velocity;

		velocity = Vector3.ClampMagnitude(velocity, 1f);

		transform.Translate(velocity, Space.World);
		
	}


}
