using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
	public GameObject objectToSpawn;
	[SerializeField] bool spawnInsideSameParent = true;
	private bool quitting = false;

	private void OnApplicationQuit ()
	{
		quitting = true;
	}

	private void OnDestroy ()
	{
		if (quitting) return;
		
		SceneHandler sceneManager = FindObjectOfType<SceneHandler>();
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
			Instantiate(objectToSpawn, transform.position, Quaternion.identity);
		}
		else
		{
			if (SceneHandler.isLoadingScene) return;
				
			GameObject g = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
			if(spawnInsideSameParent && transform.parent != null)
			{
				g.transform.parent = transform.parent;
			}
		}
	}
}