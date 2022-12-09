using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
	public bool fadeBetweenScenes = false;
	public string fadeTriggerName = "Fade";
	public Animator animator;
	public Image fadeImage;

	private string currentScene;
	public static bool isLoadingScene = false;

	private void Start ()
	{
		if (!fadeBetweenScenes) return;
		
		if (animator == null)
		{
			animator = GetComponentInChildren<Animator>();
		}

		if (fadeImage == null)
		{
			fadeImage = GetComponentInChildren<Image>();
		}
	}

	private void Awake ()
	{
		if(FindObjectsOfType<SceneHandler>().Length > 1)
		{
			Destroy(this);
		}

		isLoadingScene = false;
	}

	public void LoadScene(string sceneToLoad)
	{
		Debug.LogWarning("Scene loading attempt");
		if (isLoadingScene) return;
		
		if (!Application.CanStreamedLevelBeLoaded(sceneToLoad)) return;
		
		isLoadingScene = true;
		
		if(fadeBetweenScenes)
		{
			StartCoroutine(Fading(sceneToLoad));
		}
		else
		{
			SceneManager.LoadScene(sceneToLoad);
		}
	}

	private IEnumerator Fading(string sceneToLoad)
	{
		animator.SetTrigger(fadeTriggerName);
		
		yield return new WaitUntil(() => Math.Abs(fadeImage.color.a - 1) < .01f);
		
		SceneManager.LoadScene(sceneToLoad);
	}
}
