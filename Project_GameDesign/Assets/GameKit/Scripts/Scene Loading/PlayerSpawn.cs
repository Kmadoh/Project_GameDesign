using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
	[SerializeField] GameObject playerPrefab = null;
	[SerializeField] Vector3 initialSpawnPos = Vector3.zero;

	public void SpawnPlayer ()
	{
		//Debug.Log("Spawning player ! ");
		if(playerPrefab != null)
		{
			GameObject player;

			if (PlayerSpawnData.spawnPosition != Vector3.zero)
			{
				player = Instantiate(playerPrefab, PlayerSpawnData.spawnPosition, playerPrefab.transform.rotation);
			}
			else
			{
				player = Instantiate(playerPrefab, initialSpawnPos, playerPrefab.transform.rotation);
			}
			Life playerLife = player.GetComponent<Life>();

			if (playerLife != null)
			{
				if (PlayerSpawnData.lifeValue != 0)
				{
					playerLife.CurrentLife = PlayerSpawnData.lifeValue;
				}
				//Debug.Log("Player life : " + playerLife.currentLife);
				LifeDisplay lifeBar = FindObjectOfType<LifeDisplay>();
				if (lifeBar != null)
				{
					lifeBar.lifeToDisplay = playerLife;
					lifeBar.InitLifeBarValues();
				}

				Follow cameraFollow = FindObjectOfType<Follow>();
				if (cameraFollow != null)
				{
					cameraFollow.target = player.transform;
				}
			}
		}
		else
		{
			Debug.LogWarning("No Player Prefab Set !", gameObject);
		}

		

		
	}
	// Use this for initialization
	void Awake ()
	{
		SpawnPlayer();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
