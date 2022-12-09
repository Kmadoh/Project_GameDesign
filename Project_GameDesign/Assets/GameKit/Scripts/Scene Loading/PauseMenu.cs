using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public string pauseInputName = "Cancel";
	[SerializeField] private GameObject pauseMenu = null;
	[SerializeField] private GameObject resumeButton = null;
	private SceneHandler sceneManager;

	private EventSystem es;
	private bool isPaused = false;
	private float previousTimeScale = 1f;

	public int inputChoiceIndex;

	private void Awake ()
	{
		sceneManager = FindObjectOfType<SceneHandler>();
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
		}

		es = FindObjectOfType<EventSystem>();
		if (es == null)
		{
			Debug.Log("No EventSystem found in the scene ! WTF", gameObject);
		}
	}

	private void Update ()
	{
		if (!Input.GetButtonDown(pauseInputName) && !Input.GetKeyDown(KeyCode.Escape)) return;
		
		if(isPaused)
		{
			pauseMenu.SetActive(false);
			isPaused = false;
		}
		else
		{
			pauseMenu.SetActive(true);
			es.SetSelectedGameObject(resumeButton);
			isPaused = true;
		}
		SetTimeScale();
	}


	private void SetTimeScale()
	{
		if(isPaused)
		{
			previousTimeScale = Time.timeScale;
			Time.timeScale = .001f;
		}
		else
		{
			Time.timeScale = previousTimeScale;
		}
	}


	public void ResumeGame()
	{
		pauseMenu.SetActive(false);
		isPaused = false;
		SetTimeScale();
	}

	public void ReloadScene ()
	{
		pauseMenu.SetActive(false);
		isPaused = false;
		SetTimeScale();
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
		}
		else
		{
			string sceneToLoad = SceneManager.GetActiveScene().name;
			sceneManager.LoadScene(sceneToLoad);
		}
	}

	public void QuitGame ()
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
