using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	//[Header("Spawning options")]
	public bool shareOrientation = true;
	public bool displayDebugInfo = true;

	[System.Serializable]
	public class PrefabToSpawn
	{
		public GameObject prefab = null;
		public int weight = 1;

		public PrefabToSpawn (GameObject prefab, int weight)
		{
			this.prefab = prefab;
			this.weight = weight;
		}
	}

	public List<PrefabToSpawn> prefabToSpawn = new List<PrefabToSpawn>(1);
	public Vector3 randomMinOffset = Vector3.zero;
	public Vector3 randomMaxOffset = Vector3.zero;

	//[Header("Cooldown")]
	public bool useCooldown = false;
	public float spawnCooldown = 1f;

	//[Header("Input")]
	public bool useInput = false;
	public string spawnInputName = "Input Name (InputManager)";

	//[Header("Nested spawning")]
	public bool spawnInsideParent = false;
	public Transform parent;

	//[Header("Resources")]
	public bool requireResources = false;
	public ResourceManager resourceManager;
	public int resourceIndex = 0;
	public int resourceCostOnUse = 1;
	public bool destroyOnNoResourceLeft = false;
	
	private float nextTimeSpawn = 0f;
	public int inputChoiceIndex;

	private void Awake ()
	{
		if(useCooldown)
		{
			nextTimeSpawn = spawnCooldown;
		}	
	}
	private void Start ()
	{
		InitialDebugMessages();
	}

	private void Update ()
	{
		SpawnManagement();
	}

	private bool BetterTimeManagement()
	{
		return Time.time >= nextTimeSpawn;
	}

	private GameObject WeightedSpawn ()
	{

		int totalSize = 0;
		int currentWeight = 0;

		for (int i = prefabToSpawn.Count - 1; i >= 0; i--)
		{
			if (prefabToSpawn[i].prefab != null)
			{
				totalSize += prefabToSpawn[i].weight;
			}
		}

		if(totalSize != 0)
		{
			int randomWeight = Random.Range(0, totalSize + 1);

			foreach (PrefabToSpawn p in prefabToSpawn)
			{
				int newWeight = currentWeight + p.weight;
				if (randomWeight <= newWeight)
				{
					return p.prefab;
				}
				currentWeight = newWeight;
			}
			
			return prefabToSpawn[0].prefab;
		}
		else
		{
			return null;
		}
	}

	public void SpawnObject ()
	{
		if (prefabToSpawn.Count > 0)
		{
			GameObject gameObjectSpawned;
			GameObject weightedSpawn = WeightedSpawn();

			if(weightedSpawn == null)
			{
				return;
			}

			//Nested Spawning Check
			if (spawnInsideParent)
			{
				if (parent == null)
				{
					parent = transform;
				}

				if (shareOrientation)
				{
					Transform transform1 = parent.transform;
					gameObjectSpawned = Instantiate(weightedSpawn, transform1.position, transform1.rotation);
				}
				else
				{
					gameObjectSpawned = Instantiate(weightedSpawn, parent.transform.position, weightedSpawn.transform.rotation);
				}

				//Setting parent and local position offset
				gameObjectSpawned.transform.SetParent(parent);
				gameObjectSpawned.transform.localPosition = GetRandomOffset();
			}
			else
			{
				Instantiate(weightedSpawn, transform.position + GetRandomOffset(),
					shareOrientation ? transform.rotation : weightedSpawn.transform.rotation);
			}
		}
		else
		{
			Debug.LogError("No prefab to spawn set !", gameObject);
		}

	}

	private void SpawnManagement ()
	{
		switch (useInput)
		{
			case true when useCooldown:
			{
				if (BetterTimeManagement())
				{
					if (Input.GetButton(spawnInputName))
					{
						if (requireResources)
						{
							if (resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1))
							{
								SpawnObject();
								nextTimeSpawn = Time.time + spawnCooldown;
							}
							else if(destroyOnNoResourceLeft)
							{
								Destroy(gameObject);
							}
						}
						else
						{
							SpawnObject();
							nextTimeSpawn = Time.time + spawnCooldown;
						}
					}
				}

				break;
			}
			case true:
			{
				if (Input.GetButtonDown(spawnInputName))
				{
					if (requireResources)
					{
						if (resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1))
						{
							SpawnObject();
						}
						else if (destroyOnNoResourceLeft)
						{
							Destroy(gameObject);
						}
					}
					else
					{
						SpawnObject();
					}
				}

				break;
			}
			default:
			{
				if (useCooldown)
				{
					if (BetterTimeManagement())
					{
						if (requireResources)
						{
							if (resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1))
							{
								SpawnObject();
								nextTimeSpawn = Time.time + spawnCooldown;
							}
							else if (destroyOnNoResourceLeft)
							{
								Destroy(gameObject);
							}
						}
						else
						{
							SpawnObject();
							nextTimeSpawn = Time.time + spawnCooldown;
						}
					}
				}
				else
				{
					Debug.Log("Add either Input or Timer based spawning !", gameObject);
				}

				break;
			}
		}
	}

	private Vector3 GetRandomOffset ()
	{
		Vector3 offset = new Vector3(Random.Range(randomMinOffset.x, randomMaxOffset.x), Random.Range(randomMinOffset.y, randomMaxOffset.y), Random.Range(randomMinOffset.z, randomMaxOffset.z));
		return offset;
	}

	private void InitialDebugMessages ()
	{
		if (prefabToSpawn.Count == 0 || prefabToSpawn[0] == null)
		{
			Debug.LogWarning("No Prefab to spawn set !! Please add one", gameObject);
		}

		if (!requireResources) return;

		if (resourceManager != null) return;
		
		Debug.LogWarning("Resource Manager not referenced ! Trying to find one", gameObject);

		resourceManager = GetComponent<ResourceManager>();

		if (resourceManager == null)
		{
			Debug.LogWarning("Resource Manager not found on this GameObject ! Please add or reference one", gameObject);
		}
	}

}
