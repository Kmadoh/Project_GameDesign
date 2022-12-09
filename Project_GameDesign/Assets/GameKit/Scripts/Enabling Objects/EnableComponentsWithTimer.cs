using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentsWithTimer : MonoBehaviour
{
	public bool disableInstead = false;
	public bool displayDebugInfo = false;
	[SerializeField] float timeBeforeEnable = 3f;
	public bool disableAfter = true;
	[SerializeField] float timeBeforeDisable = 3f;
	[SerializeField] bool loop = true;
	public List<Behaviour> components = new List<Behaviour>();


	private void ComponentManagement (bool isEnabled)
	{
		for (int i = 0; i < components.Count; i++)
		{
			components[i].enabled = isEnabled;
		}
	}

	private void Start ()
	{
		StartCoroutine(EnableComponents());
	}

	private IEnumerator EnableComponents()
	{
		yield return new WaitForSeconds(timeBeforeEnable);
		ComponentManagement(!disableInstead);
		if(disableAfter)
		{
			StartCoroutine(DisableComponents());
		}
	}

	private IEnumerator DisableComponents()
	{
		yield return new WaitForSeconds(timeBeforeDisable);
		ComponentManagement(disableInstead);
		if (loop)
		{
			StartCoroutine(EnableComponents());
		}
	}
}
