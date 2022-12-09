using UnityEngine;

public static class TransformExtensions
{
	public static Vector3 ChangeXPos (this Transform transform, float x) 
	{
		Vector3 position = transform.position;
		position.x = x;
		transform.position = position;
		return position;
	}

	public static Vector3 ChangeYPos (this Transform transform, float y) 
	{
		Vector3 position = transform.position;
		position.y = y;
		transform.position = position;
		return position;
	}

	public static Vector3 ChangeZPos (this Transform transform, float z) 
	{
		Vector3 position = transform.position;
		position.z = z;
		transform.position = position;
		return position;
	}

	/// <summary>
	/// Deletes all children objects from target transform
	/// </summary>
	/// <param name="t">Transform reference</param>
	/// <returns>World Space Coordinates of rect transform</returns>
	public static void DeleteChildren(this Transform t)
	{
		foreach(Transform child in t)
		{
			Object.Destroy(child.gameObject);
		}
	}
	
		
	public static void SmoothLookAt(this Transform t, Transform targetTransform, float rotationSpeed, float delta)
	{
		Quaternion rot = t.GetLookAtRotation(targetTransform);

		t.rotation =  Quaternion.RotateTowards(t.rotation, rot, rotationSpeed * delta );
	}
	
	public static void SmoothLookAt(this Transform t, Vector3 targetPosition, float rotationSpeed, float delta)
	{
		Quaternion rot = t.GetLookAtRotation(targetPosition);

		t.rotation =  Quaternion.RotateTowards(t.rotation, rot, rotationSpeed * delta );
	}

	//Detects if a transform is facing target by threshold amount
	public static bool IsFacingTarget(this Transform transform,Transform target,float dotThreshold = 0.5f) {
		Vector3 vectorToTarget = target.position - transform.position;
		vectorToTarget.Normalize();

		float dot = UnityEngine.Vector3.Dot(transform.forward,vectorToTarget);

		return dot >= dotThreshold;
	}

	/// <summary>
	/// Find the rotation to look at a Vector3
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target">The thing to look at</param>
	/// <returns></returns>
	public static Quaternion GetLookAtRotation(this Transform self, Vector3 target)
	{
		return Quaternion.LookRotation(target - self.position);
	}

	/// <summary>
	/// Find the rotation to look at a Transform
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target">The thing to look at</param>
	/// <returns></returns>
	public static Quaternion GetLookAtRotation(this Transform self, Transform target)
	{
		return GetLookAtRotation(self, target.position);
	}

	/// <summary>
	/// Find the rotation to look at a GameObject
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target">The thing to look at</param>
	/// <returns></returns>
	public static Quaternion GetLookAtRotation(this Transform self, GameObject target)
	{
		return GetLookAtRotation(self, target.transform.position);
	}

	/// <summary>
	/// Instantly look away from a target Vector3
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target">The thing to look away from</param>
	public static void LookAwayFrom(this Transform self, Vector3 target)
	{
		self.rotation = GetLookAwayFromRotation(self, target);
	}

	/// <summary>
	/// Instantly look away from a target transform
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target">The thing to look away from</param>
	public static void LookAwayFrom(this Transform self, Transform target)
	{
		self.rotation = GetLookAwayFromRotation(self, target);
	}

	/// <summary>
	/// Instantly look away from a target GameObject
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target">The thing to look away from</param>
	public static void LookAwayFrom(this Transform self, GameObject target)
	{
		self.rotation = GetLookAwayFromRotation(self, target);
	}


	/// <summary>
	/// Find the rotation to look away from a target Vector3
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target">The thing to look away from</param>
	public static Quaternion GetLookAwayFromRotation(this Transform self, Vector3 target)
	{
		return Quaternion.LookRotation(self.position - target);
	}

	/// <summary>
	/// Find the rotation to look away from a target transform
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target">The thing to look away from</param>
	public static Quaternion GetLookAwayFromRotation(this Transform self, Transform target)
	{
		return GetLookAwayFromRotation(self, target.position);
	}

	/// <summary>
	/// Find the rotation to look away from a target GameObject
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target">The thing to look away from</param>
	public static Quaternion GetLookAwayFromRotation(this Transform self, GameObject target)
	{
		return GetLookAwayFromRotation(self, target.transform.position);
	}
}
