using UnityEngine;
using System.Collections;

public class LoginSceneController : MonoBehaviour 
{

    public void OnStartGame()
    {
        GameGlobal.LoadSceneByName(SceneNameConst.MAIN_INTERFACE);
    }
}
