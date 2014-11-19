using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class DirectionWalker : MonoBehaviour 
{
    public float speed = 2.0f;

    private Camera mainCamera;

    private NavMeshAgent navmeshAgent;

    void Awake () 
    {
        mainCamera = Camera.main;

        navmeshAgent = GetComponent<NavMeshAgent>();
	}
	
	void Update () 
    {
        float xDelta = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float zDelta = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        navmeshAgent.Move(new Vector3(xDelta, 0, zDelta));

        mainCamera.transform.position = transform.position - Vector3.forward * 1 + Vector3.up * 1.7f;
	}

    void LateUpdate ()
    {
        mainCamera.transform.LookAt(transform);
    }
}
