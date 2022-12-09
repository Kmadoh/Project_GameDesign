using UnityEngine;

public class StickToSurface : MonoBehaviour
{
	[SerializeField] private bool stickOnlyOnTop;
	public bool useTag;
	public string tagName = "Player";
	
	private Transform colTransform;

	private void OnCollisionEnter (Collision collision)
	{
		if (useTag && !collision.gameObject.CompareTag(tagName)) return;
		
		if (stickOnlyOnTop && collision.transform.position.y < transform.position.y) return;

		colTransform = collision.gameObject.transform;
		colTransform.parent = transform;
	}

	private void OnCollisionExit (Collision collision)
	{
		if (useTag && !collision.gameObject.CompareTag(tagName)) return;

		colTransform = collision.gameObject.transform;
		colTransform.parent = null;
	}
}