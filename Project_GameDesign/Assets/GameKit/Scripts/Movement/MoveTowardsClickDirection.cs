using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveTowardsClickDirection : MonoBehaviour
{
	[SerializeField] private Camera cam;

	[SerializeField] private bool lockOnX = false;
	[SerializeField] private bool lockOnY = true;
	[SerializeField] private bool lockOnZ = false;
	[SerializeField] private LayerMask clickMask = ~0;

	[SerializeField] private float moveSpeed = 10f;
	[SerializeField] private float maxSpeed = 50f;
	[SerializeField] private bool useInertia = true;
	[SerializeField] private bool keepVelocity = true;
	public bool displayDirection = true;
	[SerializeField] private Rigidbody rigid;

	[SerializeField] private LineRenderer lineRenderer;


	private Vector3 cursorPos;
	private bool isMoving = false;

	private void Awake ()
	{
		if(displayDirection && lineRenderer == null)
		{
			Debug.LogWarning("No Line Renderer set ! (Optional)", gameObject);
			lineRenderer = GetComponent<LineRenderer>();
		}
		
		if (cam == null)
		{
			Debug.LogWarning("No Camera set !", gameObject);

			cam = Camera.main;
		}

		if (rigid == null)
		{
			Debug.LogWarning("No Rigidbody set !", gameObject);
			rigid = GetComponent<Rigidbody>();
		}
	}

	private void SetTargetPos ()
	{
		if (cam == null) return;
		
		if (Input.GetMouseButton(0))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100f, clickMask))
			{
				cursorPos = hit.point;
				if (lockOnY)
				{
					cursorPos.y = transform.position.y;
				}
				else if (lockOnX)
				{
					cursorPos.x = transform.position.x;
				}
				else if (lockOnZ)
				{
					cursorPos.z = transform.position.z;
				}
				if(!useInertia)
				{
					if(keepVelocity)
					{
						rigid.velocity = (cursorPos - transform.position).normalized * rigid.velocity.magnitude;
					}
					else
					{
						rigid.velocity = Vector3.zero;
					}
				}
				transform.LookAt(cursorPos);

				isMoving = true;
			}
		}
		if (lineRenderer != null && displayDirection)
		{
			SetLineDir();
		}

	}
	private void MoveTowardsTargetPos ()
	{
		if (rigid == null) return;
		if(rigid.velocity.sqrMagnitude < maxSpeed)
		{
			rigid.AddForce(transform.forward * moveSpeed, ForceMode.Acceleration);
		}
		else
		{
			rigid.velocity = rigid.velocity.normalized * Mathf.Sqrt(maxSpeed);
		}

	}

	private void SetLineDir()
	{
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, transform.forward * 3f + transform.position);
	}

	// Update is called once per frame
	private void Update ()
	{
		SetTargetPos();
	}

	private void FixedUpdate ()
	{
		if (isMoving)
		{
			MoveTowardsTargetPos();
		}
	}
}
