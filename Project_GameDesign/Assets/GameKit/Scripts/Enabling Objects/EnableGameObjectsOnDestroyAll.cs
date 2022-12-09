using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsOnDestroyAll : MonoBehaviour
{
	public GameObject[] gameObjectsToDetect;

	public List<GameObject> gameObjectsToEnable = new List<GameObject>();
	public  bool disableInstead = false;
	public  bool displayDebugInfo = false;

	public bool searchByTag = false;
	public string tagName;

	private List<GameObject> entitiesList;

	private void Awake ()
	{
		if (searchByTag)
		{
			gameObjectsToDetect = GameObject.FindGameObjectsWithTag(tagName);
		}
		entitiesList = new List<GameObject>(gameObjectsToDetect);
		
		if (entitiesList.Count != 0) return;
		
		Debug.Log("No entities found with this tag ! Turning off this component", gameObject);
		enabled = false;

	}

	private void Update ()
	{
		if (entitiesList.Count == 0)
		{
			int arrayLength = gameObjectsToEnable.Count;
			if (arrayLength != 0)
			{
				for(int i = 0; i < arrayLength; i++)
				{
					gameObjectsToEnable[i].SetActive(!disableInstead);
				}
			}
			
			enabled = false;
		}
		else
		{
			entitiesList.RemoveAll(item => item == null);
		}
	}
}
