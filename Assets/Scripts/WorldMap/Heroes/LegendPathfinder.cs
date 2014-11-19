using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class LegendPathfinder : MonoBehaviour 
{
	//节点临近距离。如果足够接近一个节点，则开始选取下一个节点。这个值如果太小，会发生颤抖。
	private const float closeDistance = 0.5f;

	public float speed = 10f;
	public float turningSpeed = 5f;

	[HideInInspector]
	public bool hasComplete;

	private Vector3 destination;
	private Seeker seeker;

	private Path currentPath;
	private int pathIndex;
	private int pathIndexMax;

	private Vector3 targetPosition;
	private Vector3 direction;

	private Transform tran;

	private float nodeMagn;
	private float destMagn;

	private LegendShadowController shadowController;

	void Awake () 
	{
		seeker = GetComponent<Seeker>();
		tran = transform;

		destination = Vector3.zero;
		pathIndex = 0;
		targetPosition = Vector3.zero;
		
		shadowController = GetComponentInChildren<LegendShadowController>();
	}
	
	void Update () 
	{
		if (currentPath == null || hasComplete)
			return;

		Vector3 currentPosition = tran.position;
		Vector3 targetPosition = currentPath.vectorPath[pathIndex];

		float nodeSqrDistance = Vector3.SqrMagnitude(targetPosition - currentPosition);

		//如果临近节点，则选择下一个节点作为目标。如果是最后一个节点，则完成。
		if (nodeSqrDistance < closeDistance * closeDistance)
		{
			if (pathIndex == pathIndexMax - 1)
			{
				hasComplete = true;
			}
			else
			{
				pathIndex++;
			}
		}
		else
		{
			direction = (targetPosition - currentPosition).normalized;

			Vector3 velocity = direction * speed * Time.deltaTime;

			tran.Translate(velocity, Space.World);

			RotateTowards(direction);

			shadowController.UpdateRotation(velocity);
		}

	}

	public Vector3 Destination
	{
		get
		{
			return destination;
		}
		set
		{
			if (destination != value)
			{
				destination = value;
				searchPath();
			}
		}
	}

	private void searchPath()
	{
		seeker.StartPath(transform.position, destination, onSearchPathComplete);
	}

	private void onSearchPathComplete(Path path)
	{
		if (path.error)
		{
			Debug.Log(path.errorLog);
			return;
		}

		hasComplete = false;
		currentPath = path;
		pathIndex = 0;
		pathIndexMax = currentPath.vectorPath.Count;
	}

	private void RotateTowards (Vector3 dir) 
	{
		if (dir == Vector3.zero) return;
		
		Quaternion rot = tran.rotation;
		Quaternion toTarget = Quaternion.LookRotation (dir);
		
		rot = Quaternion.Lerp (rot, toTarget, turningSpeed * Time.deltaTime);
		Vector3 euler = rot.eulerAngles;
		euler.z = 0;
		euler.x = 0;
		rot = Quaternion.Euler (euler);
		
		tran.rotation = rot;
	}
}
