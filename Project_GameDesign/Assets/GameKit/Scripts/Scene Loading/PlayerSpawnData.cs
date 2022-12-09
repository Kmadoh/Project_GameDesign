using UnityEngine;

public class PlayerSpawnData : MonoBehaviour
{
	public static Vector3 spawnPosition = Vector3.zero;
	public static int lifeValue;

	public static void UpdatePlayerSpawnPos (Vector3 position, int playerLife)
	{
		lifeValue = playerLife;
		spawnPosition = position;
	}
}
