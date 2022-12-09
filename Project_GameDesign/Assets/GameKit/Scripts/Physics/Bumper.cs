using UnityEngine;

public class Bumper : MonoBehaviour
{
	[Tooltip("Force and direction of the propulsion")]
	[SerializeField] private Vector3 bumpForce = new Vector3(0f, 300f, 0f);
	[Tooltip("Do we add an additional force towards the colliding object ?")]
	[SerializeField] private bool bumpTowardsOther = false;
	[Tooltip("Additional force added towards the colliding object ?")]
	[SerializeField] private float additionalForceTowardsOther = 500f;
	[SerializeField] private bool preventInputHolding = false;
	
	[Tooltip("Do we bump only objects with a specific tag ?")]
	[SerializeField] private bool useTag = false;
	[Tooltip("Name of the tag used on collision")]
	[SerializeField] private string tagName = "Case sensitive";

	[SerializeField] private bool displayDebugInfo = true;

	private void OnCollisionEnter (Collision collision)
	{
		BumpHandler(collision.gameObject);
	}
	
	private void OnTriggerEnter (Collider other)
	{
		BumpHandler(other.gameObject);
	}
	private void BumpHandler(GameObject other)
	{
		if(useTag && !other.CompareTag(tagName)) return;
		
		Rigidbody otherRigid = other.GetComponent<Rigidbody>();
		if (otherRigid == null)
		{
			if (other.GetComponentInParent<Rigidbody>() == null)
			{
				Debug.LogWarning("No rigidbody found on colliding object ! Could not bump", gameObject);
				return;
			}
		}

		if (preventInputHolding)
		{
			Jumper j = other.GetComponent<Jumper>();

			if (j)
			{
				j.isBeingBumped = true;
			}
		}

		Vector3 toOther = other.transform.position - transform.position;
		ApplyBump(otherRigid, toOther);
	}

	private void ApplyBump (Rigidbody col, Vector3 dir)
	{
		col.velocity = Vector3.zero;

		if (bumpTowardsOther)
		{
			col.AddForce(dir.normalized * additionalForceTowardsOther, ForceMode.Impulse);
		}

		col.AddForce(bumpForce, ForceMode.Impulse);
	}
	
	private void OnDrawGizmosSelected()
	{
		if (!displayDebugInfo) return;
		
		Gizmos.color = Color.blue;

		Vector3 position = transform.position;
		Gizmos.DrawLine(position, position + bumpForce);
	}
}
