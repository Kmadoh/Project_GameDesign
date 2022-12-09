using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsOverTime : MonoBehaviour
{
	public List<GameObject> gameObjectsToEnable = new List<GameObject>();
	[SerializeField]private float cooldown = 0.2f;
	public bool revertPreviousOne = true;
	[SerializeField]private bool loop = true;
	public bool disableInstead = false;
	public bool displayDebugInfo = true;
	
	private int index = 0;

	private float timer = 0f;

	private void Awake ()
	{
		if(gameObjectsToEnable.Count == 0 || gameObjectsToEnable == null)
		{
			int children = transform.childCount;
			
			gameObjectsToEnable = new List<GameObject>(children);
			
			for (int i = 0; i < children; ++i)
			{
				gameObjectsToEnable[i] = transform.GetChild(i).gameObject;
			}
		}

		if(gameObjectsToEnable.Count != 0)
		{
			gameObjectsToEnable[0].SetActive(!disableInstead);
		}
	}
	
	private void Update ()
	{
		if (!timer.PeriodicCooldownCheck(cooldown) || gameObjectsToEnable.Count == 0) return;
		
		if(index < gameObjectsToEnable.Count-1)
		{
			if(revertPreviousOne)
			{
				gameObjectsToEnable[index].SetActive(disableInstead);
			}
			index++;
			gameObjectsToEnable[index].SetActive(!disableInstead);
		}
		else if(loop)
		{
			if (revertPreviousOne)
			{
				gameObjectsToEnable[index].SetActive(disableInstead);
			}
			index = 0;
			gameObjectsToEnable[index].SetActive(!disableInstead);
		}
	}
}
