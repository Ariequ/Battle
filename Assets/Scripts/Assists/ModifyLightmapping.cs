using UnityEngine;
using System.Collections;

public class ModifyLightmapping : MonoBehaviour
{
    public Texture2D[] lightMappingList0;
    public Texture2D[] lightMappingList1;

    private bool isZero;

    void Awake () 
    {
        isZero = true;

        UIEventListener.Get(gameObject).onClick = OnButtonClick;
	}
	
    private void OnButtonClick (GameObject go) 
    {
        Texture2D[] textureList = isZero ? lightMappingList0 : lightMappingList1;

        isZero = !isZero;

        LightmapData[] lightmapList = new LightmapData[textureList.Length];

        for (int i = 0; i < textureList.Length; ++i)
        {
            LightmapData data = new LightmapData();
            data.lightmapFar = textureList[i];
            lightmapList[i] = data;
        }

        LightmapSettings.lightmaps = lightmapList;
	}
}
