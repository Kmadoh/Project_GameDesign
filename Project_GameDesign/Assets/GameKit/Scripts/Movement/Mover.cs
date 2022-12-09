using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mover : MonoBehaviour
{
	public enum MovementType {Translation, Rigidbody, Controller}

	public MovementType movementType = MovementType.Rigidbody;
	[SerializeField] private float maximumVelocity = 5f;
	[Tooltip("Do we move along the X Axis ?")]
	public bool useXAxis = true;
	[Tooltip("Do we move along the Y Axis ?")]
	public bool useYAxis = false;
	[Tooltip("Do we move along the Z Axis ?")]
	public bool useZAxis = false;

	[Tooltip("Do we need to press any input for the object to move ?")]
	public bool requiresInput = false;
	[Tooltip("Input required to move along X Axis")]
	public string xInputName = "Horizontal";
	[Tooltip("Input required to move along Y Axis")]
	public string yInputName = "Vertical";
	[Tooltip("Input required to move along Z Axis")]
	public string zInputName = "Vertical";

	[Tooltip("Move speed on the X axis (Red)")]
	public float xSpeed = 5f;
	[Tooltip("Move speed on the Y axis (Green)")]
	public float ySpeed = 5f;
	[Tooltip("Move speed on the Z axis (Blue)")]
	public float zSpeed = 5f;

	[Tooltip("Does the object look at its movement direction ?")]
	[SerializeField] private bool lookAtDirection = false;
	[Tooltip("Does the object move according to the local or world system")]
	[SerializeField] private bool isMovementLocal = false;

	[Tooltip("Do we move against a wall ? Prevents being stuck on some surfaces")]
	public bool useWallAvoidance = false;
	[Tooltip("Bounds of wall detection")]
	[SerializeField] private Vector3 wallAvoidanceSize = new Vector3(0.3f, 1f, 0.1f);
	[Tooltip("Local offset of wall detection")]
	[SerializeField] private Vector3 wallAvoidanceOffset = new Vector3(0, 0, 0.5f);
	[Tooltip("Which Layers are considered a Wall ?")]
	[SerializeField] private LayerMask wallAvoidanceLayers = 1;

	[Tooltip("Reference to the Animator Component of the GameObject")]
	public Animator animator;

	public ParticleSystem movementParticleSystem;

	[SerializeField] float minVelocityToToggle = 0.2f;

	[Tooltip("Name of the float parameter used to animate movement direction on X axis (-1 / 1)")]
	[SerializeField] private string xDirParameterName = "xDirection";
	[Tooltip("Name of the float parameter used to animate movement direction on Y axis (-1 / 1)")]
	[SerializeField] private string yDirParameterName = "yDirection";
	[Tooltip("Name of the float parameter used to animate movement direction on Z axis (-1 / 1)")]
	[SerializeField] private string zDirParameterName = "zDirection";

	[Tooltip("Do we keep the previous orientation in memory if the GameObject doesn't move ?")]
	[SerializeField] private bool keepOrientationOnIdle = true;

	[Tooltip("Do we send to the Animator the current speed of the GameObject ?")]
	public bool trackSpeed = true;
	[Tooltip("Name of the float parameter used to animate movement depending on speed")]
	[SerializeField] private string speedParameterName = "MoveSpeed";

	[Tooltip("Do we animate specifically when the GameObject is grounded ? Allows to split ground and airborne animations")]
	public bool animateWhenGroundedOnly;
	[Tooltip("Distance between the pivot and the surface on which the GameObject moves.")]
	[SerializeField] private float collisionCheckRadius = 0.65f;
	[Tooltip("Collision check offset from the pivot point")]
	[SerializeField] private Vector3 collisionOffset = new Vector3(0, 0.1f, 0);
	[Tooltip("Name of the float parameter used to animate when grounded")]
	public LayerMask groundLayerMask = 1;
	public string groundedParameterName = "isGrounded";

	private Rigidbody r;
	private CharacterController characterController;
	public bool useGravity = true;
	public float gravityForce = 10f;
	public float maxGravityForce = 50f;
	private CustomGravity c;

	public bool displayDebugInfo = true;

	[HideInInspector] public bool canMove = true;
	[HideInInspector] public Vector3 currentSpeed = Vector3.zero;
	[HideInInspector] public int xInputChoiceIndex;
	[HideInInspector] public int yInputChoiceIndex;
	[HideInInspector] public int zInputChoiceIndex;

	private void Awake ()
	{
		if (animator == null)
		{
			if ((animator = GetComponent<Animator>()) == null)
			{
				animateWhenGroundedOnly = false;
				keepOrientationOnIdle = false;
				trackSpeed = false;
			}
		}

		if(movementType == MovementType.Rigidbody)
		{
			GetRigidbody();
		}

		if(movementType == MovementType.Controller)
		{
			GetCharacterController();
		}

		c = GetComponentInChildren<CustomGravity>();
	}

	private void FixedUpdate ()
	{
		if (movementType != MovementType.Rigidbody || !canMove) return;
		
		MoveObject(Time.fixedDeltaTime);
		HandleParticleEmission();
	}

	private void Update ()
	{
		if(movementType == MovementType.Rigidbody || !canMove) return;

		MoveObject(Time.deltaTime);
		HandleParticleEmission();
	}

	private void OnValidate ()
	{
		switch (movementType)
		{
			case MovementType.Controller:
				{
					if(characterController == null)
					{
							if(!TryGetComponent<CharacterController>(out characterController))
							{
								characterController = gameObject.AddComponent<CharacterController>();
								Debug.LogWarning("Adding a Character Controller to your GameObject !");
							}
					}

					if (r != null)
					{
						Debug.LogWarning("Setting the Rigidbody to kinematic on your GameObject since it could conflict with the Character Controller !");
						r.isKinematic = true;
					}
					
					characterController.enabled = true;
				}
			break;

			case MovementType.Rigidbody:
				{
					if (r == null)
					{
						if (!TryGetComponent<Rigidbody>(out r))
						{
							r = gameObject.AddComponent<Rigidbody>();
							Debug.LogWarning("Adding a Rigidbody to your GameObject !");
						}
					}

					if (characterController != null)
					{
						Debug.LogWarning("Disabling the Character Controller on your GameObject since it's no longer needed !");
						r.isKinematic = false;
						characterController.enabled = false;
					}
				}	
			break;

			case MovementType.Translation:
				{
					if (characterController != null)
					{
						Debug.LogWarning("Disabling the Character Controller on your GameObject since it's no longer needed !");
						characterController.enabled = false;
					}
				}
			break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void GetRigidbody()
	{
		r = GetComponent<Rigidbody>();

		if (r != null)
		{
			return;
		}

		Debug.LogError("This GameObject requires a Rigidbody component to move !");
		r = gameObject.AddComponent<Rigidbody>();
	}
	
	private void GetCharacterController()
	{
		characterController = GetComponent<CharacterController>();

		if (characterController != null) return;

		Debug.LogError("This GameObject requires a Character Controller component to move !");
		characterController = gameObject.AddComponent<CharacterController>();

	}

	private void HandleParticleEmission()
	{
		if (movementParticleSystem == null)
		{
			return;
		}

		Vector3 hvMagnitude = movementType == MovementType.Rigidbody
			? Vector3.Scale(r.velocity, currentSpeed.normalized)
			: currentSpeed;
		
		bool isMoving = hvMagnitude.magnitude >= minVelocityToToggle;
		if(isMoving)
		{
			if(!movementParticleSystem.isPlaying)
			{
				movementParticleSystem.Play();
			}
		}
		else if(movementParticleSystem.isPlaying)
		{
			movementParticleSystem.Stop();
		}
		
	}

	private void GetSpeed ()
	{
		if (requiresInput)
		{
			currentSpeed.x = useXAxis ? Input.GetAxis(xInputName) : currentSpeed.x;
			currentSpeed.y = useYAxis ? Input.GetAxis(yInputName) : currentSpeed.y;
			currentSpeed.z = useZAxis ? Input.GetAxis(zInputName) : currentSpeed.z;
			
			if (currentSpeed.magnitude > 1f)
			{
				currentSpeed.Normalize();
			}

			currentSpeed.x *= useXAxis ? xSpeed : 1;
			currentSpeed.y *= useYAxis ? ySpeed : 1;
			currentSpeed.z *= useZAxis ? zSpeed : 1;
		}
		else
		{
			currentSpeed.x = useXAxis ? xSpeed : currentSpeed.x;
			currentSpeed.y = useYAxis ? ySpeed : currentSpeed.y;
			currentSpeed.z = useZAxis ? zSpeed : currentSpeed.z;
		}
	}
	
	private void MoveObject(float delta)
	{
		GetSpeed();

		if (lookAtDirection && currentSpeed != Vector3.zero)
		{
			transform.forward = currentSpeed;
		}

		//if ((!useWallAvoidance || transform.WallCheck(wallAvoidanceOffset, wallAvoidanceSize, wallAvoidanceLayers)) && useWallAvoidance) return;
		if (useWallAvoidance && Physics.CheckBox(transform.position + transform.TransformDirection(wallAvoidanceOffset), wallAvoidanceSize, transform.rotation, wallAvoidanceLayers, QueryTriggerInteraction.Ignore)) return;
		
		if (animator != null)
		{
			UpdateAnimatorValues(currentSpeed);
		}

		switch (movementType)
		{
			case MovementType.Translation:
			{

				Vector3 movement = isMovementLocal ? transform.TransformDirection(currentSpeed) : currentSpeed;
				transform.Translate(movement * delta, Space.World);
			}
				break;

			case MovementType.Rigidbody:
			{
				Vector3 movement = isMovementLocal ? transform.TransformDirection(currentSpeed) : currentSpeed;
				r.AddForce(movement * delta * 500f, ForceMode.Acceleration);

				Vector3 clampedVelocity = r.velocity;


				if (r.useGravity || c != null)
				{
					clampedVelocity.y = 0f;
					clampedVelocity = Vector3.ClampMagnitude(clampedVelocity, maximumVelocity);
					clampedVelocity.y = r.velocity.y;
				}
				else
				{
					clampedVelocity = Vector3.ClampMagnitude(clampedVelocity, maximumVelocity);
				}

				r.velocity = clampedVelocity;
			}
				break;

			case MovementType.Controller:
			{
				if(useGravity)
				{
					Vector3 movement = isMovementLocal ? transform.TransformDirection(currentSpeed) : currentSpeed;
					characterController.SimpleMove(movement * delta * 25f);
				}
				else
				{
					HandleCharacterControllerGravity();
					Vector3 movement = isMovementLocal ? transform.TransformDirection(currentSpeed) : currentSpeed;
					characterController.Move(movement * delta);
				}
			}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void HandleCharacterControllerGravity()
	{
		if(characterController.isGrounded && currentSpeed.y < 0f)
		{
			currentSpeed.y = 0f;
		}
		else
		{
			currentSpeed.y -= gravityForce * Time.deltaTime;
			currentSpeed.y = Mathf.Clamp(currentSpeed.y, -maxGravityForce, Mathf.Infinity);
		}
	}


	#region Debug
	private void OnDrawGizmosSelected ()
	{
		if(displayDebugInfo)
		{
			Gizmos.matrix = transform.localToWorldMatrix;

			if (useWallAvoidance)
			{
				Gizmos.color = Physics.CheckBox(transform.position + transform.TransformDirection(collisionOffset), wallAvoidanceSize, transform.rotation, wallAvoidanceLayers, QueryTriggerInteraction.Ignore)
					? Color.red
					: Color.green;

				Gizmos.DrawWireCube(wallAvoidanceOffset, wallAvoidanceSize * 2f);
			}

			Vector3 transformPosition = transform.position;
			if (animator != null && animateWhenGroundedOnly)
			{
				Gizmos.color =
					Helper.GroundCheck(transformPosition, collisionOffset, transform, collisionCheckRadius, groundLayerMask)
						? Color.blue
						: Color.red;

				Gizmos.DrawWireSphere(collisionOffset, collisionCheckRadius);
			}

			Gizmos.color = Color.green;

			Gizmos.matrix = Matrix4x4.identity;

			if (requiresInput)
			{
				
				if (isMovementLocal)
				{
					Gizmos.DrawLine(transformPosition, transformPosition + transform.TransformDirection(currentSpeed.normalized));
				}
				else
				{
					Gizmos.DrawLine(transformPosition, transformPosition + currentSpeed.normalized);
				}
			}
			else
			{
				Vector3 moveDir = new Vector3(useXAxis?xSpeed:0f, useYAxis?ySpeed:0f, useZAxis?zSpeed:0f);
				if (isMovementLocal)
				{
					Gizmos.DrawLine(transformPosition, transformPosition + transform.TransformDirection(moveDir.normalized));
				}
				else
				{
					Gizmos.DrawLine(transformPosition, transformPosition + moveDir.normalized);
				}
			}
		}		
	}
	#endregion

	#region Animation

	private void SetIdleValues(string parameterName)
	{
		if (animator.GetFloat(parameterName) > 0f)
		{
			animator.SetFloat(parameterName, 0.1f);
		}
		else if (animator.GetFloat(parameterName) < 0f)
		{
			animator.SetFloat(parameterName, -0.1f);
		}
	}

	private void UpdateAnimatorValues (Vector3 speed)
	{
		if (keepOrientationOnIdle)
		{
			if (speed != Vector3.zero)
			{
				ApplyDirectionValues();
			}
			else
			{
				if (useXAxis)
				{
					SetIdleValues(xDirParameterName);
				}
				if (useYAxis)
				{
					SetIdleValues(yDirParameterName);
				}
				if (useZAxis)
				{
					SetIdleValues(zDirParameterName);
				}
			}
		}
		else
		{
			ApplyDirectionValues();
		}

		if (trackSpeed)
		{
			animator.SetFloat(speedParameterName, speed.magnitude);
		}

		if (!animateWhenGroundedOnly)
		{
			return;
		}
		
		bool isGrounded = Helper.GroundCheck(transform.position, collisionOffset, transform , collisionCheckRadius, groundLayerMask);
		
		animator.SetBool(groundedParameterName, isGrounded);
	}

	private void ApplyDirectionValues ()
	{
		if (useXAxis && xSpeed != 0)
		{
			animator.SetFloat(xDirParameterName, currentSpeed.x / xSpeed);
		}
		if (useYAxis && ySpeed != 0)
		{
			animator.SetFloat(yDirParameterName, currentSpeed.y / ySpeed);
		}
		if (useZAxis && zSpeed != 0)
		{
			animator.SetFloat(zDirParameterName, currentSpeed.z / zSpeed);
		}
	}
	#endregion
}