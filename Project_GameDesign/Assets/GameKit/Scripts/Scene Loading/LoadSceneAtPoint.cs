using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAtPoint : MonoBehaviour
{
	public Vector3 spawnPosition;

	[Tooltip("The name of the scene we should load")]
	public string sceneToLoad;
	[Tooltip("Do we use a tag for trigger detection ?")]
	public bool useTag;
	public string tagName;
	public int sceneChoiceIndex;

	private void Start ()
	{
		if (sceneToLoad != null) return;
		
		sceneToLoad = SceneManager.GetActiveScene().name;
		Debug.Log("No scene to load, using current scene", gameObject);
	}

	private void OnTriggerEnter (Collider other)
	{
		if (!enabled) return;

		if (useTag && !other.CompareTag(tagName)) return;

		SceneHandler sceneManager = FindObjectOfType<SceneHandler>();
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
			return;
		}

		Life life = other.GetComponent<Life>();
		int lifeCount = 1;
		if (life != null)
		{
			lifeCount = life.CurrentLife;
		}

		PlayerSpawnData.UpdatePlayerSpawnPos(spawnPosition, lifeCount);

		sceneManager.LoadScene(sceneToLoad);
	}
}