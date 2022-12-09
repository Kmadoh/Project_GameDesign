using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class AddForceWhileInTrigger : MonoBehaviour
{

	[SerializeField] private Vector3 addedForce = Vector3.up;
	[SerializeField] private bool isLocal = false;
	[SerializeField] private bool overrideForce = false;

	public bool useTag = false;
	public string tagName = "Player";

	private void OnTriggerStay (Collider other)
	{
		if (useTag && !other.CompareTag(tagName)) return;
		
		ApplyForce(other);
	}

	private void ApplyForce(Collider other)
	{
		Rigidbody rb = other.GetComponent<Rigidbody>();

		if (!rb) return;
		
		Vector3 appliedForce = addedForce;
		if (isLocal)
		{
			appliedForce = appliedForce.WorldToLocalSpace(transform);
		}

		rb.AddForce(appliedForce, overrideForce ? ForceMode.VelocityChange : ForceMode.Force);
	}
}