using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Life))]
public class LoadSceneOnDestroy : MonoBehaviour
{
	[Header("Needs a Life Component to work properly !")]
	public string sceneToLoad;
	public static bool quitting = false;
	public int sceneChoiceIndex;

	private void Start ()
	{
		if (sceneToLoad != null) return;
		
		sceneToLoad = SceneManager.GetActiveScene().name;
		Debug.Log("No scene to load, using current scene", gameObject);
	}

	public void LoadScene ()
	{
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
