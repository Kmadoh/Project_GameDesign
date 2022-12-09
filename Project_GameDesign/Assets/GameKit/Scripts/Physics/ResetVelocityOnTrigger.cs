using UnityEngine;

public class ResetVelocityOnTrigger : MonoBehaviour
{
	[SerializeField] private bool resetOwnVelocity = true;
	[SerializeField] private bool resetOthersVelocity = false;
	public bool useTag = true;
	public  string tagName = "Player";

	private void ResetVelocity(Collider other)
	{
		if (resetOthersVelocity)
		{
			Rigidbody rigid = other.GetComponent<Rigidbody>();
			if (rigid != null)
			{
				rigid.velocity = Vector3.zero;
			}
		}

		if (!resetOwnVelocity) return;
		{
			Rigidbody rigid = GetComponent<Rigidbody>();
			if (rigid != null)
			{
				rigid.velocity = Vector3.zero;
			}
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if (useTag && !other.CompareTag(tagName)) return;
		
		ResetVelocity(other);
	}
}
