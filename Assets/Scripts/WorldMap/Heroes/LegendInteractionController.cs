using UnityEngine;
using System.Collections;

public class LegendInteractionController : MonoBehaviour
{
	LegendCameraController cameraController;
	Vector3 mouseCurrentPosition;
	Vector3 _startPosition;
	Vector3 _endPosition;
	bool isMouseDown;

	void Start ()
	{
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			onMouseDown ();
		}
		else if (Input.GetMouseButton (0))
		{
			onMouseMove ();
		}
		else if (Input.GetMouseButtonUp (0))
		{
			onMouseUp ();
		}
	}

	private void onMouseUp ()
	{
		isMouseDown = false;
	}

	private void onMouseDown ()
	{
		isMouseDown = true;
		_startPosition = Input.mousePosition;
	}
	
	private void onMouseMove ()
	{
		if (isMouseDown)
		{
			float mouseMoveDistance = Vector3.Distance (_startPosition, Input.mousePosition);
			if (mouseMoveDistance >= LegendConst.MIN_MOUSEMOVE_DISTANCE)
			{
				Vector3 offset = _endPosition - Input.mousePosition;
				offset.z = offset.y;
				offset.y = 0;
//				cameraController.MoveCameraXZWithOffset (0.1f * offset);
//				cameraController.isFollowingTarget = false;
			}
			
			_endPosition = Input.mousePosition;
		}
	}

	private void initColliders()
	{

	}
}
