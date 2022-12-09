using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Patrol : MonoBehaviour
{
	[System.Serializable]
	public class PatrolPoint
	{
		[Tooltip("Not gameplay related, so you can find/sort them better")]
		public string pointName = "Patrol Point";
		[Tooltip("Position of the patrol point")]
		public Vector3 pointPosition = Vector3.zero;
		[Tooltip("How long should the GameObject wait when reaching a point")]
		public float waitTimeAtPoint = 0f;
		[Tooltip("Color of the point in the editor view")]
		public Color pointColor = Color.blue;
	}

	public List<PatrolPoint> patrolPoints = new List<PatrolPoint>(1);
	[Tooltip("Are the coordinates in local or world space ?")]
	public bool areCoordinatesLocal = false;
	[Tooltip("Movement speed towards each patrol point")]
	[SerializeField] float moveSpeed = 5f;
	[Tooltip("Minimum distance where a point is considered reached")]
	[SerializeField] float minDistance = 0.3f;
	[Tooltip("Upon reaching last patrol point, does the GameObject go back to the first one ?")]
	[SerializeField] bool loop = true;


	public bool lookAtTargetPos = false;
	public bool smoothLookAt = false;
	public float rotateSpeed = 5f;
	public bool lockXRotation = true;
	public bool lockYRotation = false;
	public bool lockZRotation = true;

	float timer;
	int index = 0;
	bool waiting = false;
	bool hasReachedDestination = false;

	void Start ()
	{
		if (patrolPoints.Count > 0)
		{
			timer = patrolPoints[0].waitTimeAtPoint;
		}

		if(areCoordinatesLocal && transform.parent == null)
		{
			GameObject parent = new GameObject();
			parent.name = gameObject.name + " Patrol parent";
			parent.transform.position = transform.position;

			transform.parent = parent.transform;
		}
	}

	void WaitTime ()
	{
		timer -= Time.deltaTime;
		if (lookAtTargetPos)
		{
			LookAtTargetPos();
		}
		if (timer <= 0f)
		{
			waiting = false;
			timer = patrolPoints[index].waitTimeAtPoint;
		}
	}

	private void OnDrawGizmosSelected ()
	{
		for(int i = 0; i< patrolPoints.Count; i++)
		{
			if(areCoordinatesLocal)
			{
				if(transform.parent == null)
				{
					Debug.DrawLine(transform.position + patrolPoints[i].pointPosition + Vector3.down * 0.25f, transform.position + patrolPoints[i].pointPosition + Vector3.up * 0.25f, patrolPoints[i].pointColor);
					Debug.DrawLine(transform.position + patrolPoints[i].pointPosition + Vector3.left * 0.25f, transform.position + patrolPoints[i].pointPosition + Vector3.right * 0.25f, patrolPoints[i].pointColor);
					Debug.DrawLine(transform.position + patrolPoints[i].pointPosition + Vector3.back * 0.25f, transform.position + patrolPoints[i].pointPosition + Vector3.forward * 0.25f, patrolPoints[i].pointColor);

					if (patrolPoints.Count > 1)
					{
						if (i < patrolPoints.Count - 1)
						{
							Debug.DrawLine(transform.position + patrolPoints[i].pointPosition, transform.position + patrolPoints[i + 1].pointPosition, patrolPoints[i].pointColor);
						}
						else if (loop)
						{
							Debug.DrawLine(transform.position + patrolPoints[i].pointPosition, transform.position + patrolPoints[0].pointPosition, patrolPoints[i].pointColor);
						}
					}
				}
				else
				{
					Debug.DrawLine(transform.parent.position + patrolPoints[i].pointPosition + Vector3.down * 0.25f, transform.parent.position + patrolPoints[i].pointPosition + Vector3.up * 0.25f, patrolPoints[i].pointColor);
					Debug.DrawLine(transform.parent.position + patrolPoints[i].pointPosition + Vector3.left * 0.25f, transform.parent.position + patrolPoints[i].pointPosition + Vector3.right * 0.25f, patrolPoints[i].pointColor);
					Debug.DrawLine(transform.parent.position + patrolPoints[i].pointPosition + Vector3.back * 0.25f, transform.parent.position + patrolPoints[i].pointPosition + Vector3.forward * 0.25f, patrolPoints[i].pointColor);

					if (patrolPoints.Count > 1)
					{
						if (i < patrolPoints.Count - 1)
						{
							Debug.DrawLine(transform.parent.position + patrolPoints[i].pointPosition, transform.parent.position + patrolPoints[i + 1].pointPosition, patrolPoints[i].pointColor);
						}
						else if (loop)
						{
							Debug.DrawLine(transform.parent.position + patrolPoints[i].pointPosition, transform.parent.position + patrolPoints[0].pointPosition, patrolPoints[i].pointColor);
						}
					}
				}
			}
			else
			{
				Debug.DrawLine(patrolPoints[i].pointPosition + Vector3.down * 0.25f, patrolPoints[i].pointPosition + Vector3.up * 0.25f, patrolPoints[i].pointColor);
				Debug.DrawLine(patrolPoints[i].pointPosition + Vector3.left * 0.25f, patrolPoints[i].pointPosition + Vector3.right * 0.25f, patrolPoints[i].pointColor);
				Debug.DrawLine(patrolPoints[i].pointPosition + Vector3.back * 0.25f, patrolPoints[i].pointPosition + Vector3.forward * 0.25f, patrolPoints[i].pointColor);

				if (patrolPoints.Count > 1)
				{
					if (i < patrolPoints.Count - 1)
					{
						Debug.DrawLine(patrolPoints[i].pointPosition, patrolPoints[i + 1].pointPosition, patrolPoints[i].pointColor);
					}
					else if (loop)
					{
						Debug.DrawLine(patrolPoints[i].pointPosition, patrolPoints[0].pointPosition, patrolPoints[i].pointColor);
					}
				}
			}
			
		}
	}

	void LookAtTargetPos()
	{
		if (smoothLookAt)
		{
			Quaternion targetRotation;
			if (areCoordinatesLocal)
			{
				targetRotation = Quaternion.LookRotation(transform.parent.position + patrolPoints[index].pointPosition - transform.position);
			}
			else
			{
				targetRotation = Quaternion.LookRotation(patrolPoints[index].pointPosition - transform.position);
			}

			if (lockXRotation)
			{
				targetRotation.x = 0f;
			}
			if (lockYRotation)
			{
				targetRotation.y = 0f;
			}
			if (lockZRotation)
			{
				targetRotation.z = 0f;
			}
			// Smoothly rotate towards the target point.
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
		}
		else
		{
			if (areCoordinatesLocal)
			{
				transform.LookAt(transform.parent.position + patrolPoints[index].pointPosition);

			}
			else
			{
				transform.LookAt(patrolPoints[index].pointPosition);
			}
			
		}
	}

	void MoveTowardsNextPoint ()
	{
		if (!waiting)
		{
			if (areCoordinatesLocal)
			{
				if (Vector3.Distance(transform.localPosition, patrolPoints[index].pointPosition) > minDistance)
				{
					hasReachedDestination = false;
				}
				else
				{
					hasReachedDestination = true;
				}
			}
			else
			{
				if (Vector3.Distance(transform.position, patrolPoints[index].pointPosition) > minDistance)
				{
					hasReachedDestination = false;
				}
				else
				{
					hasReachedDestination = true;
				}
			}

			if (!hasReachedDestination)
			{
				if (lookAtTargetPos)
				{
					LookAtTargetPos();
				}

				if(areCoordinatesLocal)
				{
					transform.localPosition = Vector3.MoveTowards(transform.localPosition, patrolPoints[index].pointPosition, moveSpeed * Time.deltaTime);
				}
				else
				{
					transform.position = Vector3.MoveTowards(transform.position, patrolPoints[index].pointPosition, moveSpeed * Time.deltaTime);
				}
			}
			else
			{
				if (loop)
				{
					if (index == patrolPoints.Count - 1)
					{
						index = 0;
					}
					else
					{
						index++;
					}
				}
				else
				{
					if (index < patrolPoints.Count - 1)
					{
						index++;
					}
				}
				waiting = true;
			}
		}
		else
		{
			WaitTime();
		}

	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		MoveTowardsNextPoint();
	}
}
