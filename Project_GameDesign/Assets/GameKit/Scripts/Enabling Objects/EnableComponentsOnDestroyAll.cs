using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentsOnDestroyAll : MonoBehaviour
{
	public GameObject[] gameObjectsToDetect;
	public List<Behaviour> components = new List<Behaviour>();
	public bool disableInstead = false;
	public bool searchByTag = false;
	public string tagName;

	public bool displayDebugInfo = false;
	[HideInInspector] public List<GameObject> entitiesList;

	private void Awake ()
	{
		if (searchByTag)
		{
			gameObjectsToDetect = GameObject.FindGameObjectsWithTag(tagName);
		}
		entitiesList = new List<GameObject>(gameObjectsToDetect);
		
		if (entitiesList.Count != 0) return;
		
		Debug.Log("No entities found with this tag or inside 'Game Objects To Detect' ! Turning off this component", gameObject);
		enabled = false;
	}


	private void Update ()
	{
		if (entitiesList.Count == 0)
		{
			foreach (Behaviour behaviour in components)
			{
				behaviour.enabled = !disableInstead;
			}

			enabled = false;
		}
		else
		{
			entitiesList.RemoveAll(item => item == null);
		}
	}
}
