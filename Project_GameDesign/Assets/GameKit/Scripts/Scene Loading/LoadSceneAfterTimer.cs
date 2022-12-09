using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterTimer : MonoBehaviour
{
	[Tooltip("The name of the scene we should load")]
	public string sceneToLoad = "Scene Name In Assets (Case Sensitive)";
	[Tooltip("Do we use a tag for trigger detection ?")]
	[SerializeField] private float timeBeforeLoading = 2f;

	public int sceneChoiceIndex;

	private void Start ()
	{
		if (sceneToLoad == null)
		{
			sceneToLoad = SceneManager.GetActiveScene().name;
			Debug.Log("No scene to load, using current scene", gameObject);
		}
		else
		{
			StartCoroutine(LoadSceneAfter());
		}
	}

	private IEnumerator LoadSceneAfter()
	{
		yield return new WaitForSeconds(timeBeforeLoading);

		SceneHandler sceneManager = FindObjectOfType<SceneHandler>();
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
		}
		else
		{
			sceneManager.LoadScene(sceneToLoad);
		}
	}
}