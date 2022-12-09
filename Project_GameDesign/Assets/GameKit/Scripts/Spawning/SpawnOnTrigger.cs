using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
	public bool displayDebugInfo = true;
	//[Header("Collision options")]
	public bool onlyOnce = false;
	public bool useTagOnTrigger = false;
	public string tagName = "Player";

	//[Header("Spawning Options")]
	public bool shareOrientation = true;
	public GameObject[] prefabToSpawn = new GameObject[1];
	public Vector3 randomMinOffset = Vector3.zero;
	public Vector3 randomMaxOffset = Vector3.zero;

	//[Header("Nested spawning")]
	public bool spawnInsideParent = false;
	public bool spawnInsideCollidingObject = false;
	public Transform parent;

	//[Header("Resources")]
	public bool requireResources = false;
	public ResourceManager resourceManager;
	public int resourceIndex = 0;
	public int resourceCostOnUse = 1;

	//[Header("Input")]
	public bool requireInput = false;
	public string inputName = "";

	bool hasSpawned = false;
	public int inputChoiceIndex;

	private Vector3 GetRandomOffset ()
	{
		Vector3 offset = new Vector3(Random.Range(randomMinOffset.x, randomMaxOffset.x), Random.Range(randomMinOffset.y, randomMaxOffset.y), Random.Range(randomMinOffset.z, randomMaxOffset.z));
		return offset;
	}

	private void Start ()
	{
		if (!requireResources) return;

		if (resourceManager != null) return;
		
		if((resourceManager = FindObjectOfType<ResourceManager>()) == null)
		{
			Debug.LogError("No Resource Manager in Scene !");
		}
	}

	private void SpawnObject()
	{
		int randomIndex = Random.Range(0, prefabToSpawn.Length);

		if (parent == null && spawnInsideCollidingObject == false)
		{
			parent = transform;
		}

		if (spawnInsideParent)
		{
			GameObject gameObjectSpawned = Instantiate(prefabToSpawn[randomIndex], parent.transform.position, shareOrientation ? parent.transform.rotation : prefabToSpawn[randomIndex].transform.rotation);
			gameObjectSpawned.transform.parent = parent;
			gameObjectSpawned.transform.localPosition = GetRandomOffset();
		}
		else
		{
			Instantiate(prefabToSpawn[randomIndex], transform.position + GetRandomOffset(), shareOrientation ? transform.rotation : prefabToSpawn[randomIndex].transform.rotation);
		}
		
	}

	private void ResourceCheck ()
	{
		if (requireResources)
		{
			if (!resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1)) return;
			
			SpawnObject();
			hasSpawned = true;
		}
		else
		{
			SpawnObject();
			hasSpawned = true;
		}
	}

	private void TagCheck(Collider other)
	{
		if (spawnInsideCollidingObject)
		{
			parent = other.transform;
		}

		if (useTagOnTrigger)
		{
			Rigidbody r = other.GetComponent<Rigidbody>();
			if (r == null)
			{
				r = other.GetComponentInParent<Rigidbody>();
			}

			if (r == null) return;
			
			if (spawnInsideCollidingObject)
			{
				parent = r.transform;
			}

			if (r.gameObject.CompareTag(tagName))
			{
				ResourceCheck();
			}
		}
		else
		{
			ResourceCheck();
		}
	}

	private void SpawnCheck(Collider other)
	{
		if (onlyOnce)
		{
			if (hasSpawned)
			{
				return;
			}

			TagCheck(other);
		}
		else
		{
			TagCheck(other);
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if(requireInput)
		{
			return;
		}

		SpawnCheck(other);
	}

	private void OnTriggerStay (Collider other)
	{
		if(!requireInput)
		{
			return;
		}

		if (Input.GetButtonDown(inputName))
		{
			SpawnCheck(other);
		}
	}
}
