using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
	//[Header("Follow Position Parameters")]
	[Tooltip("The target transform this object should follow")]
	public Transform target;
	public bool followOnXAxis = true;
	public bool followOnYAxis = true;
	public bool followOnZAxis = true;
	[Tooltip("The time it takes to this object to reach the followed object")]
	[Range(0f,1f)]
	public float smoothTime = 0.3f;
	[SerializeField] float maxDistance = 15f;

	//[Space]

	//[Header("Follow Orientation Parameters")]
	public bool shareOrientation;
	public bool rotateOnXAxis = true;
	public bool rotateOnYAxis = true;
	public bool rotateOnZAxis = true;
	public float rotateSpeed = 10f;

	Vector3 velocity = Vector3.zero;

	Vector3 GetTargetPos()
	{
		Vector3 targetPos = transform.position;
		if(followOnXAxis)
		{
			targetPos.x = target.position.x;
		}
		if (followOnYAxis)
		{
			targetPos.y = target.position.y;
		}
		if (followOnZAxis)
		{
			targetPos.z = target.position.z;
		}

		return targetPos;
	}

	Vector3 GetTargetRot ()
	{
		Vector3 targetRot = transform.eulerAngles;
		if (rotateOnXAxis)
		{
			targetRot.x = target.forward.x;
		}
		if (rotateOnYAxis)
		{
			targetRot.y = target.forward.y;
		}
		if (rotateOnZAxis)
		{
			targetRot.z = target.forward.z;
		}

		return targetRot;
	}

	void MoveTowards()
	{
		if(target)
		{
			if(Vector3.Distance(transform.position, GetTargetPos()) > maxDistance)
			{
				transform.position = GetTargetPos();
			}
			else
			{
				transform.position = Vector3.SmoothDamp(transform.position, GetTargetPos(), ref velocity, smoothTime);
				if (shareOrientation)
				{
					//transform.forward = target.forward;
					Vector3 newDir = Vector3.RotateTowards(transform.forward, GetTargetRot(), rotateSpeed * Time.deltaTime, 0f);
					transform.rotation = Quaternion.LookRotation(newDir);
				}
			}
		}
		else
		{
			Debug.Log("No target to follow !!", gameObject);
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		MoveTowards();
	}
}
