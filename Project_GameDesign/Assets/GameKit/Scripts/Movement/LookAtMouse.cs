using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
	private  enum AxisToUse { X, Y, Z };
	[Tooltip("Which axis do we use for rotation ?")]
	[SerializeField] private AxisToUse rotationAxis = AxisToUse.Y;
	[Tooltip("Rotation speed towards mouse position, in angles per second")]
	[SerializeField] private float rotationSpeed = 360;
	[Tooltip("Offset from hit point")]
	[SerializeField] private Vector3 offset = Vector3.zero;

	[SerializeField] private LayerMask layerMask;

	private Camera cam;
	private void LookAtMouseCursor()
	{
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (!Physics.Raycast(ray, out hit, Single.MaxValue, layerMask)) return;
		
		Vector3 cursorPos = hit.point;

		switch (rotationAxis)
		{
			case AxisToUse.X:
				cursorPos.x = transform.position.x;
				break;

			case AxisToUse.Y:
				cursorPos.y = transform.position.y;
				break;

			case AxisToUse.Z:
				cursorPos.z = transform.position.z;
				break;

			default:
				throw new ArgumentOutOfRangeException();
		}

		cursorPos += offset;
		
		transform.SmoothLookAt(cursorPos, rotationSpeed, Time.deltaTime);

	}

	private void Awake()
	{
		cam = Camera.main;
	}

	private void Update ()
	{
		LookAtMouseCursor();
	}
}
