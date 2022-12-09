using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsInRange : MonoBehaviour
{
	public bool disableInstead = false;
	public bool displayDebugInfo = false;
	public float range;
	public Transform specificTarget;
	public bool findWithTag = true;
	public string targetTagName;
	public List<GameObject> gameObjectsToEnable = new List<GameObject>();
	[Tooltip("Which layers do we want to affect ?")]
	public LayerMask layerMask = ~0;
	public bool revertWhenOutOfRange = true;

	bool isInRange = false;
	int targetsInRange = 0;
	float distanceToTarget;

	private void ComponentManagement (bool isEnabled)
	{
		for (int i = 0; i < gameObjectsToEnable.Count; i++)
		{
			gameObjectsToEnable[i].SetActive(isEnabled);

		}
	}

	// Update is called once per frame
	private void Update ()
	{
		targetsInRange = 0;
		if (specificTarget != null)
		{
			distanceToTarget = Vector3.Distance(transform.position, specificTarget.position);
			if (distanceToTarget <= range)
			{
				targetsInRange = 1;
			}
		}
		else
		{
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, layerMask);
			if (hitColliders.Length >= 1)
			{
				for (int i = 0; i < hitColliders.Length; i++)
				{
					if (targetTagName != null)
					{
						if (hitColliders[i].CompareTag(targetTagName))
						{
							targetsInRange++;
						}
					}
					else
					{
						targetsInRange++;
					}

				}
			}
		}

		if (targetsInRange > 0)
		{
			if (isInRange) return;
			
			ComponentManagement(!disableInstead);
			isInRange = true;
		}
		else
		{
			if (!isInRange) return;
			
			if (revertWhenOutOfRange)
			{
				ComponentManagement(disableInstead);
			}
			isInRange = false;
		}

	}
}
