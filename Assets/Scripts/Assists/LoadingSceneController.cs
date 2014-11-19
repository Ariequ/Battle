using UnityEngine;
using System.Collections;

public class LoadingSceneController : MonoBehaviour 
{
	public UISlider loadingProgressBar;

	private AsyncOperation loadingOperation;

	void Start () 
	{
		this.loadingOperation = Application.LoadLevelAsync (GameGlobal.LoadingSceneName);
	}

	void Update () 
	{
		if (! loadingOperation.isDone)
		{
			loadingProgressBar.value = loadingOperation.progress;
		}
	}
}
