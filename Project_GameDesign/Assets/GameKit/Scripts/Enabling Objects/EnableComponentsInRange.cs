using UnityEngine;
using System.Collections.Generic;

public class EnableComponentsInRange : MonoBehaviour
{
	public float range;
	public Transform specificTarget;
	public string targetTagName;
	public bool disableInstead = false;
	public List<Behaviour> components = new List<Behaviour>();
	[Tooltip("Which layers do we want to affect ?")]
	public LayerMask layerMask;
	public bool findWithTag = true;
	public bool revertWhenOutOfRange = true;
	public bool displayDebugInfo = true;

	bool isInRange = false;
	int targetsInRange = 0;
	float distanceToTarget;

	void ComponentManagement(bool isEnabled)
	{
		foreach (Behaviour t in components)
		{
			t.enabled = isEnabled;
		}
	}
	
	void Update ()
	{
		targetsInRange = 0;
		if (specificTarget != null)
		{
			distanceToTarget = Vector3.Distance(transform.position, specificTarget.position);
			if(distanceToTarget <= range)
			{
				targetsInRange = 1;
			}
		}
		else
		{
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, layerMask);
			if (hitColliders.Length >= 1)
			{
				foreach (Collider t in hitColliders)
				{
					if(targetTagName != null && !t.CompareTag(targetTagName))
					{
						continue;
					}

					targetsInRange++;
				}
			}
		}

		if(targetsInRange > 0)
		{
			if (isInRange) return;
			ComponentManagement(!disableInstead);
			isInRange = true;
		}
		else
		{
			if (!isInRange) return;
			
			if(revertWhenOutOfRange)
			{
				ComponentManagement(disableInstead);
			}
			isInRange = false;
		}

	}

	private void OnDrawGizmosSelected()
	{
		if (!displayDebugInfo) return;

		Gizmos.color = disableInstead ? Color.red : Color.green;
		
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
