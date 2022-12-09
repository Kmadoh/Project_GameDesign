using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
	public enum InvertAnimationType { Rotation, Animation, None};

	[Tooltip("The constant gravity force applied to the Rigidbody")]
	public Vector3 baseGravityForce = new Vector3(0f,-9.81f,0f);
	public Vector3 secondaryGravityForce = new Vector3(0f,9.81f,0f);
	[Tooltip("Maximum velocity of the Rigidbody")]
	public float maxVelocity = 50f;

	[Tooltip("Do we invert gravity on Input ?")]
	public bool invertOnInput = true;
	[Tooltip("Input used for gravity inverting")]
	public string inputName = "Jump";
	[Tooltip("Is gravity change instant ?")]
	public bool instantGravityChangeOnInput = false;
	[Tooltip("Do we invert Jumping Direction ?")]
	public bool invertJumpingDirection = false;

	[Tooltip("Only Allow Gravity Inversion when grounded")]
	public bool onlyWhenGrounded = false;
	[Tooltip("Estimated distance between the pivot and the ground when Inverting")]
	public float collisionCheckDistance = 0.65f;

	public InvertAnimationType invertAnimationType = InvertAnimationType.Rotation;
	public Transform transformToInvert;
	public Vector3 invertedRotation = new Vector3(-180, 0, 0);
	public Vector3 normalRotation = new Vector3(0, 0, 0);
	public string gravityChangeBoolName = "inverted";
	public Animator animator;

	private Rigidbody rigid;
	private Jumper jump;

	private bool isInverted = false;
	[HideInInspector] public Vector3 currentGravityForce;

	[HideInInspector] public bool showForces = true;
	[HideInInspector] public bool showGravity = true;
	[HideInInspector] public bool showGroundCheck = true;
	[HideInInspector] public bool showAnimation = true;

	public int inputChoiceIndex;

	// Use this for initialization
	void Start ()
	{
		currentGravityForce = baseGravityForce;

		rigid = GetComponent<Rigidbody>();
		if(rigid != null)
		{
			rigid.useGravity = false;
		}
		else
		{
			Debug.LogError("Add a Rigidbody without useGravity for this to work !", gameObject);
		}

		if(invertJumpingDirection)
		{
			jump = GetComponent<Jumper>();
			if(jump == null)
			{
				Debug.LogError("No Jumper Component found on this object !", gameObject);
				invertJumpingDirection = false;
			}
		}

		if(transformToInvert == null)
		{
			transformToInvert = transform;
		}
	}
	
	private void ApplyGravity()
	{
		Vector3 baseGravity = baseGravityForce.normalized;
		baseGravity = Vector3.Scale(baseGravity, rigid.velocity);
		if(Mathf.Abs(baseGravity.magnitude) < maxVelocity)
		{
			rigid.AddForce(currentGravityForce, ForceMode.Acceleration);
		}
	}

	private bool GroundCheck ()
	{
		RaycastHit hit;
		Vector3 dir = currentGravityForce.normalized;

		return Physics.Raycast(transform.position, dir, out hit, collisionCheckDistance);
	}

	private void InvertGravity()
	{
		if (!Input.GetButtonDown(inputName) || rigid == null) return;
		
		currentGravityForce = currentGravityForce == baseGravityForce ? secondaryGravityForce : baseGravityForce;
		
		rigid.velocity = instantGravityChangeOnInput ? Vector3.zero : rigid.velocity * 0.9f;

		if (invertJumpingDirection)
		{
			jump.jumpForce.y *= -1f;
		}

		isInverted = !isInverted;

		InvertAnimation();
	}

	private void InvertAnimation()
	{
		switch (invertAnimationType)
		{
			case InvertAnimationType.Animation:
				{
					if (animator != null || TryGetComponent<Animator>(out animator))
					{
						animator.SetBool(gravityChangeBoolName, isInverted);
					}
				}
			break;

			case InvertAnimationType.Rotation:
			{
				transformToInvert.localEulerAngles = isInverted ? invertedRotation : normalRotation;
			}
			break;
			
			case InvertAnimationType.None:
				break;
			
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void Update()
	{
		if (!invertOnInput) return;

		if (onlyWhenGrounded && !GroundCheck()) return;

		InvertGravity();
	}

	private void FixedUpdate ()
	{
		if(rigid != null)
		{
			ApplyGravity();
		}
	}
}
