using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
	[Tooltip("The transform this object should move towards")]
	[SerializeField] private Transform targetTransform;
	[Tooltip("The target tag this object should look for")]
	public  string targetTag = "Player";
	[Tooltip("Does the object look at its target ?")]
	public bool lookAtTarget;
	[Tooltip("Does the object look at its target ?")]
	[SerializeField] private float rotationSpeed = 360f;
	[Tooltip("Movement speed towards the target")]
	[SerializeField] private float moveSpeed;
	[Tooltip("Minimal distance between target and this object")]
	[SerializeField] private float minDistance = 0.2f;

	private void Awake ()
	{
		if(targetTransform == null)
		{
			targetTransform = GameObject.FindGameObjectWithTag(targetTag).transform;
		}
	}
	
	private void Update ()
	{
		if (!targetTransform || !(Vector3.Distance(transform.position, targetTransform.position) > minDistance)) return;
		
		transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);
		
		if (!lookAtTarget) return;
		
		transform.SmoothLookAt(targetTransform, rotationSpeed, Time.deltaTime);
	}
}