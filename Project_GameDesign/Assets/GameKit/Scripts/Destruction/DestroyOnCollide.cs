using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{
	[Tooltip("Do we check the collided object's tag before destroying this object ?")]
	public bool useTag = true;
	[Tooltip("Tag we should use if useTag is set to true")]
	public string tagName;

	private void OnCollisionEnter (Collision collision)
	{
		if(useTag && !collision.gameObject.CompareTag(tagName))
		{
			return;
		}

		Destroy(gameObject);
	}
}
