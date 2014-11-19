using UnityEngine;
using System.Collections;

public class SpaceWalker : MonoBehaviour 
{
    private const float MAX_DISTANCE = 120f;

    public bool autoWalk = false;

    public float speed = 3f;

    private float direction = 1f;

    void Awake () 
    {
	
	}
	
	void Update () 
    {
        if (!autoWalk)
        {

            float xDelta = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            transform.Translate(new Vector3(xDelta, 0, 0));

            if (transform.position.x < 0)
            {
                transform.position = new Vector3(0, 5f, 0);
            }
            else if (transform.position.x > MAX_DISTANCE)
            {
                transform.position = new Vector3(MAX_DISTANCE, 5f, 0);
            }
        }
        else
        {

            transform.Translate(new Vector3(speed * Time.deltaTime * direction, 0, 0));

            if (transform.position.x > MAX_DISTANCE)
            {
                direction = -1f;
                transform.position = new Vector3(MAX_DISTANCE, 5f, 0);
            }

            if (transform.position.x < 0)
            {
                direction = 1f;
                transform.position = new Vector3(0, 5f, 0);
            }
        }

	}

}
