using UnityEngine;

public class LoadSceneOnTrigger : MonoBehaviour
{
	[Tooltip("The name of the scene we should load")]
	public string sceneToLoad = "";
	[Tooltip("Do we use a tag for trigger detection ?")]
	public bool useTag = true;
	public string tagName = "Player";
	public int sceneChoiceIndex;

	private void OnTriggerEnter (Collider other)
	{
		if (!enabled) return;

		if (useTag && !other.CompareTag(tagName)) return;

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