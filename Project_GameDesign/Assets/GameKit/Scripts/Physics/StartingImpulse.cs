using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StartingImpulse : MonoBehaviour
{
	[SerializeField] private Vector3 impulsePower = new Vector3(0f, 0f, 40f);
	[SerializeField] private bool isLocal = true;
	[SerializeField] private Rigidbody rigid;

	private void Awake()
	{
		if (!rigid && !TryGetComponent<Rigidbody>(out rigid))
		{
			Debug.LogError("No Rigidbody on this GameObject ! Please add one", gameObject);
			return;
		}

		if (isLocal)
		{
			rigid.AddRelativeForce(impulsePower, ForceMode.Impulse);
			return;
		}

		rigid.AddForce(impulsePower, ForceMode.Impulse);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;

		Vector3 dir = isLocal ? impulsePower.WorldToLocalSpace(transform) : impulsePower;
		Vector3 pos = transform.position;
		Gizmos.DrawLine(pos, pos + dir);
	}
}
