using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public string sceneToLoad = "Scene Name To Load";

	private SceneHandler sceneManager;
	public int sceneChoiceIndex;

	private void Awake ()
	{
		sceneManager = FindObjectOfType<SceneHandler>();
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
		}
	}

	public void LoadScene()
	{
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
		}
		else
		{
			sceneManager.LoadScene(sceneToLoad);
		}
	}

	public void QuitGame()
	{
		#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlaying)
			{
				UnityEditor.EditorApplication.isPlaying = false;
			}
		#endif

		Application.Quit();
	}
}