using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnableComponentsOnTrigger : MonoBehaviour
{
	public enum RevertOn { InputUp, SecondPress, TriggerExit, Timer, Never }
	public List<Behaviour> components = new List<Behaviour>();

	public bool useTag = false;
	public string tagName = "Case Sensitive";

	public bool disableInstead = false;
	public RevertOn revertOn = RevertOn.TriggerExit;

	public float revertAfterCooldown = 1f;
	//public bool revertOnTriggerExit = false;

	public bool requireResources = false;
	public ResourceManager resourceManager;
	public string resourceID;
	public int resourceCostOnUse = 1;

	public bool requireInput = false;
	public string inputName = "Case Sensitive";

	public bool displayDebugInfo = false;
	private bool wasActivated = false;

	private bool isInside = false;

	private void Update ()
	{
		if (revertOn == RevertOn.InputUp)
		{
			if (Input.GetButtonUp(inputName))
			{
				if (wasActivated)
				{
					EnableComponents(false);
				}
			}
		}

		if (!isInside) return;
		
		if (requireInput)
		{
			InputCheck();
		}
	}

	private void EnableComponents (bool enable)
	{
		if (wasActivated == !enable)
		{
			if (disableInstead)
			{
				for (int i = 0; i < components.Count; i++)
				{
					components[i].enabled = !enable;
				}
			}
			else
			{
				for (int i = 0; i < components.Count; i++)
				{
					components[i].enabled = enable;
				}
			}
		}
		wasActivated = enable;
	}
	private void ResourceCheck ()
	{
		if (requireResources && !resourceManager.ChangeDicoResourceAmount(resourceID, resourceCostOnUse * -1)) return;

		if (revertOn == RevertOn.Timer && !wasActivated)
		{
			StopAllCoroutines();
			StartCoroutine(RevertAfterTime());
		}
		EnableComponents(true);
	}

	private IEnumerator RevertAfterTime ()
	{
		yield return new WaitForSeconds(revertAfterCooldown);

		EnableComponents(false);
	}

	private void InputCheck ()
	{
		if (!Input.GetButtonDown(inputName)) return;
		
		if (revertOn == RevertOn.SecondPress)
		{
			if (wasActivated)
			{
				EnableComponents(false);
			}
			else
			{
				ResourceCheck();
			}
		}
		else
		{
			ResourceCheck();
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if (useTag)
		{
			Rigidbody r = other.GetComponent<Rigidbody>();
			if (r == null)
			{
				r = other.GetComponentInParent<Rigidbody>();
			}

			if (r == null) return;
			if (!r.CompareTag(tagName)) return;
			
			isInside = true;
			
			if (!requireInput)
			{
				ResourceCheck();
			}
		}
		else
		{
			isInside = true;
			if (!requireInput)
			{
				ResourceCheck();
			}
		}
	}

	private void OnTriggerExit (Collider other)
	{
		if (revertOn == RevertOn.TriggerExit)
		{
			if (useTag)
			{
				Rigidbody r = other.GetComponent<Rigidbody>();
				if (r == null)
				{
					r = other.GetComponentInParent<Rigidbody>();
				}
				if (r.CompareTag(tagName))
				{
					EnableComponents(false);
				}
			}
			else
			{
				EnableComponents(false);
			}
		}
		else
		{
			isInside = false;
		}
	}
}