using UnityEngine;
using System.Collections;

public class ZoomCameraController : MonoBehaviour
{
    private const int SCALE = 2;
    public UIWidget picture;
    public float zoomSpeed = 1f;
    private bool isZooming = false;
    private float screenRatio;
    private Vector3 cameraPosition;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        camera.fieldOfView = 90;
        camera.isOrthoGraphic = false;
        transform.localPosition = new Vector3(0, 0, -picture.height / 2);     
    }
    
    void Update()
    {
        if (isZooming)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, cameraPosition, Time.deltaTime * zoomSpeed);
            if (Vector3.SqrMagnitude(transform.localPosition - cameraPosition) < 5f)
                isZooming = false;
        }
    }

    public void SetFocusPosition(Vector3 focusPosition)
    {
        if (focusPosition != Vector3.zero)
        {
            cameraPosition = caculateTargetCameraPosition(focusPosition);
        }
        else
        {
            cameraPosition = new Vector3(0, 0, -picture.height / 2);
        }
        isZooming = true;
        
    }

    private Vector3 caculateTargetCameraPosition(Vector3 foucusPosition)
    {
        float z = - picture.height / 2 / SCALE;

        float x = Mathf.Max(foucusPosition.x, z * Screen.width / Screen.height);

        float y = Mathf.Min(foucusPosition.y, -z);

        return new Vector3(x, y, z);
    }

}
