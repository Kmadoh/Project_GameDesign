using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsWithTimer : MonoBehaviour
{
	public bool disableInstead = false;
	public bool displayDebugInfo = true;
	[SerializeField] private float timeBeforeEnable = 3f;
	public bool disableAfter = true;
	[SerializeField] private float timeBeforeDisable = 3f;
	[SerializeField] private bool loop = true;
	public List<GameObject> gameObjectsToEnable = new List<GameObject>();

	private void ComponentManagement (bool isEnabled)
	{
		for (int i = 0; i < gameObjectsToEnable.Count; i++)
		{
			gameObjectsToEnable[i].SetActive(isEnabled);
		}
	}

	private void Start ()
	{
		StartCoroutine(EnableComponents());
	}

	private  IEnumerator EnableComponents ()
	{
		yield return new WaitForSeconds(timeBeforeEnable);
		ComponentManagement(!disableInstead);
		if (disableAfter)
		{
			StartCoroutine(DisableComponents());
		}
	}

	private  IEnumerator DisableComponents ()
	{
		yield return new WaitForSeconds(timeBeforeDisable);
		ComponentManagement(disableInstead);
		if (loop)
		{
			StartCoroutine(EnableComponents());
		}
	}
}
