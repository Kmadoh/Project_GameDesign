using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
	public Transform targetTransform;
	public float rotationSpeed = 360f;
	public bool useTag;
	public string tagName;

	private void Awake ()
	{
		if (useTag && targetTransform == null)
		{
			targetTransform = GameObject.FindGameObjectWithTag(tagName).transform;
		}
	}
	
	private void Update ()
	{
		if (!targetTransform) return;
		
		transform.SmoothLookAt(targetTransform, rotationSpeed, Time.deltaTime);
	}
}