using UnityEngine;
using System.Collections;

public class BattleFieldWindow : MonoBehaviour 
{
	private UISprite area;

	private Camera _uiCamera;
	private Camera _innerCamera;

	private bool initialized = false;

    private void Initialize ()
    {
        area = GetComponent<UISprite>();
        
        _uiCamera = NGUITools.FindInParents<Camera>(transform);
        _innerCamera = ((GameObject) GameObject.FindGameObjectWithTag(Tags.INNER_CAMERA)).GetComponent<Camera>();

        initialized = true;
    }

	public void Display(bool state)
	{
        if (!initialized)
        {
            Initialize();
        }

        if (!state)
		{
			_innerCamera.pixelRect = new Rect();
		}
        else
        {
            float ratio = Screen.width / 1920f;
            
            Vector3 areaPosition = _uiCamera.WorldToScreenPoint(area.transform.position);
            Rect viewRect = new Rect(
                areaPosition.x, 
                areaPosition.y, 
                area.width * ratio,
                area.height * ratio 
                );
            
            _innerCamera.pixelRect = viewRect;
        }
	}
}
