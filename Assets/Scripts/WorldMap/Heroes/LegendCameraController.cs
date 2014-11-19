using UnityEngine;
using System.Collections;

public class LegendCameraController : MonoBehaviour
{
	public Rect mapRect;
	public GameObject focusGameObject;
	Vector3 currentPosition;
	private float orthographicSize;
	private Vector3 currentTargetPosition;

//	// The target we are following
//	public Transform target;
	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	public float height = 5.0f;
	// How much we 
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	public Transform target;
	private Camera m_camera;
	public bool isFollowingTarget = true;
		
	void LateUpdate ()
	{
		// Early out if we don't have a target
		if (!target || !isFollowingTarget)
		{
			return;
		}
		
		// Calculate the current rotation angles
		var wantedRotationAngle = target.eulerAngles.y;
		var wantedHeight = target.position.y + height;
		
		var currentRotationAngle = m_camera.transform.eulerAngles.y;
		var currentHeight = m_camera.transform.position.y;
		
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		m_camera.transform.position = target.position;
		m_camera.transform.position -= currentRotation * Vector3.forward * distance;
		
		// Set the height of the camera
		Vector3 temp = m_camera.transform.position;
		temp.y = currentHeight;

		m_camera.transform.position = adjustCameraPosition (temp);

		// Always look at the target
//		m_camera.transform.LookAt (target);
	}

	void Start ()
	{
		orthographicSize = Camera.main.orthographicSize;
		currentTargetPosition = focusGameObject.transform.position;
		m_camera = Camera.main;
		currentPosition = m_camera.transform.position;
	}
	
//	// Update is called once per frame
//	void Update ()
//	{
////		currentPosition = Camera.main.transform.position;
////		if (currentPosition.x != currentTargetPosition.x || currentPosition.z != currentTargetPosition.z)
////		{
////			currentPosition.x += (currentTargetPosition.x - currentPosition.x) / ExploreSceneConst.CAMERA_MOVE_SPEED;
////			currentPosition.z += (currentTargetPosition.z - currentPosition.z) / ExploreSceneConst.CAMERA_MOVE_SPEED;
////			Camera.main.transform.position = adjustCameraPosition (currentPosition);
////		}
//	}

	public void StopCameraMove ()
	{
		Debug.Log ("stop move");
		currentTargetPosition = currentPosition;
	}

	public void MoveCameraXZToPosition (Vector3 targetPosition)
	{
		currentTargetPosition = targetPosition;

		Debug.Log (currentTargetPosition.ToString ());
	}

	public void SetCameraPositionXZ (Vector3 position)
	{
		position.y = Camera.main.transform.position.y;
		Camera.main.transform.position = adjustCameraPosition (position);
	}

	public void MoveCameraXZWithOffset (Vector3 offSet)
	{
//		Debug.Log("offset");
		Vector3 targetPosition = m_camera.transform.position + offSet;
		m_camera.transform.position = adjustCameraPosition (targetPosition);
	}

	private Vector3 adjustCameraPosition (Vector3 currentPosition)
	{
		if (Camera.main == null)
		{
			return currentPosition;
		}

		Vector3 adjustedResult = currentPosition;
		float orthographHeight = Camera.main.aspect * orthographicSize;

		if (currentPosition.x < mapRect.x + orthographHeight)
		{
			adjustedResult.x = mapRect.x + orthographHeight;
		}

		if (currentPosition.z > mapRect.height - orthographicSize - distance)
		{
			adjustedResult.z = mapRect.height - orthographicSize - distance;
		}

		if (currentPosition.x > mapRect.width - orthographHeight)
		{
			adjustedResult.x = mapRect.width - orthographHeight;
		}

		if (currentPosition.z < mapRect.y + orthographicSize  - distance)
		{
			adjustedResult.z = mapRect.y + orthographicSize - distance;
		}

		return adjustedResult;
	}
}