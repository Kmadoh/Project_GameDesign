using System.Collections.Generic;
using UnityEngine;

public class LoadSceneOnDestroyAll : MonoBehaviour
{
	public GameObject[] entities;
	private List<GameObject> entitiesList;

	public bool searchByTag;
	public string tagName;

	public string sceneToLoad;

	private const float CheckFrequency = 1f;
	private float timer;
	public int sceneChoiceIndex;

	private void Awake ()
	{
		if (searchByTag)
		{
			entities = GameObject.FindGameObjectsWithTag(tagName);
		}
		entitiesList = new List<GameObject>(entities);
		
		if (entitiesList.Count != 0) return;
		
		Debug.Log("No entities found with this tag ! Turning off this component", gameObject);
		enabled = false;

	}
	
	private void Update ()
	{
		if (!timer.PeriodicCooldownCheck(CheckFrequency)) return;
		
		CheckPool();
	}

	private void CheckPool()
	{
		if (entitiesList.Count == 0)
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
			enabled = false;
		}
		else
		{
			entitiesList.RemoveAll(item => item == null);
		}
	}
}
