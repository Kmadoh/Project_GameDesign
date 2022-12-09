using UnityEngine;

public static class RigidbodyExtensions
{
	public static void ChangeDirection(this Rigidbody rb, Vector3 direction)
	{
		rb.velocity = direction.normalized * rb.velocity.magnitude;
	}
	
	public static void ChangeDirection(this Rigidbody rb, Vector3 direction, float minMagnitude)
	{
		float magnitude = rb.velocity.magnitude;

		if (magnitude < minMagnitude) magnitude = minMagnitude;
		
		rb.velocity = direction.normalized * magnitude;
	}
	
	public static void ChangeDirection(this Rigidbody rb, float maxMagnitude, Vector3 direction)
	{
		float magnitude = rb.velocity.magnitude;

		if (magnitude > maxMagnitude) magnitude = maxMagnitude;
		
		rb.velocity = direction.normalized * magnitude;
	}
	
	public static void ChangeDirection(this Rigidbody rb, float maxMagnitude, float minMagnitude, Vector3 direction)
	{
		float magnitude = rb.velocity.magnitude;

		if (magnitude > maxMagnitude) magnitude = maxMagnitude;
		else if (magnitude < minMagnitude) magnitude = minMagnitude;
		
		rb.velocity = direction.normalized * magnitude;
	}

	public static void NormalizeVelocity(this Rigidbody rb, float magnitude = 1)
	{
		rb.velocity = rb.velocity.normalized * magnitude;
	}
}