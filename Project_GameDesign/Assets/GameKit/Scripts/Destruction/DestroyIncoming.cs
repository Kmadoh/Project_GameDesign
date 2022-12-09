using UnityEngine;

public class DestroyIncoming : MonoBehaviour
{
	public bool useTag;
	public string tagName;

	private void OnTriggerEnter (Collider other)
	{
		if (useTag && !other.CompareTag(tagName))
		{
			return;
		}

		Destroy(other.gameObject);
	}
}