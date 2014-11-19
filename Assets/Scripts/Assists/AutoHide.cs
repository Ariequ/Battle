using UnityEngine;
using System.Collections;

public class AutoHide : MonoBehaviour 
{
    public float delay = 1f;

    public void Show()
    {
        StopAllCoroutines();

        gameObject.SetActive(true);

        StartCoroutine(Hide());
    }
	
    IEnumerator Hide()
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }
}
