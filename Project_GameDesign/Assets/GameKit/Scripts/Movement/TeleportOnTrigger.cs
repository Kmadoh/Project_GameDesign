using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
	public Transform teleportPoint = null;
	[SerializeField] private Vector3 offset = Vector3.zero;
	
	public bool useTag = false;
	public string tagName = "Player";

	public bool requireInput = false;
	public string inputName = "Fire1";

	public bool displayTeleportPoint = true;
	public int inputChoiceIndex = 0;

	private void Awake ()
	{
		if (inputName != "Case Sensitive" || !requireInput) return;
		
		requireInput = false;
		Debug.Log("No Input set ! Setting requireInput to false", gameObject);
	}

	private void Teleport(Collider other)
	{
		Rigidbody r = other.GetComponent<Rigidbody>();
		if (r == null)
		{
			r = other.GetComponentInParent<Rigidbody>();
		}

		if (useTag && !r.CompareTag(tagName)) return;

		r.MovePosition(teleportPoint.position + offset);
	}
	private void OnTriggerEnter (Collider other)
	{
		if(!requireInput)
		{
			Teleport(other);
		}
	}

	private void OnTriggerStay (Collider other)
	{
		if (!requireInput) return;
		
		if(Input.GetButtonDown(inputName))
		{
			Teleport(other);
		}
	}

	public void ToggleDisplayTeleportPoint(bool toggle)
	{
		displayTeleportPoint = toggle;
	}

	public void CreateTeleportPoint()
	{
		teleportPoint = new GameObject("Teleport Point (" + gameObject.name + ")").transform;
		teleportPoint.position = transform.position;
	}

	public void SelectTeleportPoint()
	{
		
	}

	private void OnDrawGizmos()
	{
		if (!teleportPoint || !displayTeleportPoint) return;
		
		Gizmos.color = Color.green;

		Vector3 transformPos = transform.position;
		Vector3 teleportPointPos = teleportPoint.position;
		
		Gizmos.DrawIcon(transformPos,"GameKit/Icon_TeleportIn.png", true);
		Gizmos.DrawIcon(teleportPointPos,"GameKit/Icon_TeleportOut.png", true);
		
		if(offset != Vector3.zero) Gizmos.DrawWireSphere(teleportPointPos + offset, 0.1f);
		
		Gizmos.DrawLine(transformPos, teleportPointPos);
		
	}
}
