using System;
using UnityEngine;

public class ResourceModifOnTrigger : MonoBehaviour
{
	public bool useTag = false;
	public string tagName = "Player";

	[Tooltip("Do we use the Resource Manager Component of the colliding object or a fixed one ?")]
	public bool useCollidingResourceManager = false;
	public ResourceManager resourceManager = null;
	public string resourceID = "ID";
	[SerializeField] private int resourceAmount = 0;

	public GameObject spawnedFXOnTrigger = null;
	[SerializeField] private float fxLifeTime = 3f;
	[SerializeField] private bool destroyAfter = false;

	public bool displayDebugInfo = true;

	private void Start()
    {
	    if (resourceManager != null) return;
	    

	    resourceManager = FindObjectOfType<ResourceManager>();
	    if (resourceManager == null)
	    {
		    Debug.LogWarning("No Resource Manager referenced ! Please add one", gameObject);
	    }
    }

	private void ModifyResource(Collider other)
	{
		ResourceManager manager;

		if(useCollidingResourceManager)
		{
			manager = other.gameObject.GetComponent<ResourceManager>();

			if(manager == null && resourceManager != null)
			{
				manager = resourceManager;
			}
		}
		else
		{
			manager = resourceManager;
		}

		manager.ChangeDicoResourceAmount(resourceID, resourceAmount);

		if(spawnedFXOnTrigger != null)
		{
			GameObject fX = Instantiate(spawnedFXOnTrigger, transform.position, Quaternion.identity);
			Destroy(fX, fxLifeTime);
		}

		if(destroyAfter)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if(useTag)
		{
			Rigidbody rigid = other.GetComponent<Rigidbody>();
			if (rigid == null)
			{
				rigid = other.GetComponentInParent<Rigidbody>();
			}

			if (rigid.CompareTag(tagName))
			{
				ModifyResource(other);
			}
		}
		else
		{
			ModifyResource(other);
		}
	}
}
