using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Jumper : MonoBehaviour
{
	//[Header("Jump")]
	[Tooltip("Input name used for jumping (From InputManager)")]
	public string jumpInputName = "Jump";
	[Tooltip("Force applied when jumping")]
	public Vector3 jumpForce = new Vector3(0f, 10f, 0f);
	[Range(0f, 1f)]
	public float coyoteTimeDuration = 0.25f;
	float coyoteTimer = 0f;
	[Tooltip("Which Layers are considered as ground ?")]
	[SerializeField] private LayerMask jumpLayerMask = 1;

	[Range(0f, 10f)]
	[Tooltip("Under which velocity do we consider the jump to end ?")]
	[SerializeField] private float jumpEndYVelocity = 1f;

	[Range(0f, 5f)]
	[Tooltip("Additional gravity multiplier applied after jump ended")]
	[SerializeField] private float fallGravityMultiplier = 3f;
	
	[Range(0f, 5f)]
	[Tooltip("Additional gravity multiplier applied during jump if button was released")]
	[SerializeField] private float lowJumpGravityMultiplier = 3f;

	//[Header("Air Jump")]
	[Tooltip("Amount of consecutive jumps allowed")]
	public int jumpAmount = 1;
	[Tooltip("Force applied when jumping")]
	public Vector3 airJumpForce = new Vector3(0f, 7f, 0f);
	[Tooltip("Do we reset the velocity on each consecutive jump ?")]
	public bool resetVelocityOnAirJump = false;

	//[Header("Collision Check")]
	public bool useCollisionCheck = true;
	[Tooltip("Offset from the pivot when detecting collision with the ground")]
	[SerializeField] private Vector3 collisionOffset = new Vector3(0, 0.4f, 0);
	[Tooltip("Estimated width of the GameObject")]
	[SerializeField] private float collisionCheckRadius = 0.5f;

	//[Header("FX")]
	[Tooltip("Visual/Sound FX Instantiated on jump")]
	public GameObject jumpFX = null;
	[Tooltip("Visual/Sound FX Instantiated on air jump")]
	public GameObject airJumpFX = null;
	[Tooltip("Offset from the pivot when Instantiating FX")]
	[SerializeField] private Vector3 fxOffset = Vector3.zero;
	[Tooltip("Lifetime of the Instantiated FX. 0 = Do not destroy")]
	[SerializeField] private float timeBeforeDestroyFX = 3f;


	//[Header("Animation")]
	[Tooltip("Reference to the Animator Component")]
	public Animator animator;
	[Tooltip("Name of the Trigger parameter called when jumping")]
	public string jumpTriggerName = "jump";
	[Tooltip("Name of the Trigger parameter called when air jumping. If you don't have any, set it to the value of jumpTriggerName")]
	public string airJumpTriggerName = "jump";

	[HideInInspector] public int currentJumpAmount;

	private Rigidbody rigid;

	public bool isBeingBumped = false;

	private CustomGravity c;
	public float characterHeight = 1.2f;
	private WallJump wallJump;
	private bool jumpRequest = false;
	private bool jumpFlag = false;

	public bool displayDebugInfo = true;
	public int inputChoiceIndex;


	private void Start ()
	{
		wallJump = GetComponent<WallJump>();

		rigid = GetComponent<Rigidbody>();
		if ( rigid == null)
		{
			GameObject o;
			rigid = (o = gameObject).AddComponent<Rigidbody>();
			Debug.Log("The GameObject doesn't have a Rigidbody !", o);
		}

		if(animator == null)
		{
			animator = GetComponent<Animator>();
		}

		if(airJumpFX == null && jumpFX != null)
		{
			airJumpFX = jumpFX;
		}

		c = GetComponentInChildren<CustomGravity>();
		currentJumpAmount = jumpAmount;
	}

	private bool GroundCheck ()
	{
		if (!useCollisionCheck) return true;
		
		Vector3 checkPosition = transform.position + collisionOffset;
		if(c != null && c.currentGravityForce.y > 0)
		{
			checkPosition += Vector3.up * characterHeight;
		}

		if (!Physics.CheckSphere(checkPosition, collisionCheckRadius, jumpLayerMask)) return false;
		
		isBeingBumped = false;

		currentJumpAmount = jumpAmount;
		return true;

	}

	private void GravityIncreases()
	{
		Vector3 gravity;
		if (c != null)
		{
			gravity = c.enabled ? c.currentGravityForce : Physics.gravity;

			if(gravity.y <= 0f)
			{
				if (rigid.velocity.y < jumpEndYVelocity)
				{
					rigid.velocity += gravity * (fallGravityMultiplier - 1) * Time.deltaTime;
				}
				else if ((rigid.velocity.y > jumpEndYVelocity && !Input.GetButton(jumpInputName)) || isBeingBumped)
				{
					rigid.velocity += gravity * (lowJumpGravityMultiplier - 1) * Time.deltaTime;
				}
			}
			else
			{
				if (rigid.velocity.y > jumpEndYVelocity)
				{
					rigid.velocity += gravity * (fallGravityMultiplier - 1) * Time.deltaTime;
				}
				else if ((rigid.velocity.y < jumpEndYVelocity && !Input.GetButton(jumpInputName)) || isBeingBumped)
				{
					rigid.velocity += gravity * (lowJumpGravityMultiplier - 1) * Time.deltaTime;
				}
			}
		}
		else
		{
			gravity = Vector3.up * Physics.gravity.y;
			if (rigid.velocity.y < jumpEndYVelocity)
			{
				rigid.velocity += gravity * (fallGravityMultiplier - 1) * Time.deltaTime;
			}
			else if ((rigid.velocity.y > jumpEndYVelocity && !Input.GetButton(jumpInputName)) || isBeingBumped)
			{
				rigid.velocity += gravity * (lowJumpGravityMultiplier - 1) * Time.deltaTime;
			}
		}
	}

	private bool WallJumpCheck()
	{
		if (wallJump == null) return true;

		Transform transform1 = transform;

		if (Physics.Raycast(transform1.position + collisionOffset, Vector3.down, out _, wallJump.minimalHeightAllowedToWallJump, wallJump.wallJumpLayerMask))
		{
			return true;
		}

		return !Physics.Raycast(transform1.position + wallJump.collisionOffset, transform1.forward * -1f, out _, 2f);

	}

	private void FixedUpdate ()
	{
		if(jumpRequest)
		{
			if (WallJumpCheck())
			{
				if (GroundCheck() || CoyoteTimeCheck())
				{
					if (animator != null)
					{
						animator.SetTrigger(jumpTriggerName);
					}

					jumpFlag = true;
					SpawnFX(jumpFX);
					Jump(jumpForce);
				}
				else if (jumpAmount > 1 && currentJumpAmount > 1)
				{
					if (animator != null)
					{
						animator.SetTrigger(airJumpTriggerName);
					}

					SpawnFX(airJumpFX);
					Jump(airJumpForce);

					currentJumpAmount--;
				}
			}
			jumpRequest = false;
		}

		GravityIncreases();
	}
	

	private void Update ()
	{
		if (Input.GetButtonDown(jumpInputName))
		{
			jumpRequest = true;
		}

		if (!GroundCheck()) return;
		
		coyoteTimer = Time.time;
		jumpFlag = false;

	}

	private bool CoyoteTimeCheck()
	{
		return Time.time <= coyoteTimer + coyoteTimeDuration && !jumpFlag;
	}

	private void SpawnFX(GameObject spawnedFX)
	{
		if (spawnedFX == null) return;
		
		GameObject fx = Instantiate(spawnedFX, transform.position + fxOffset, Quaternion.identity);

		if (timeBeforeDestroyFX > 0f)
		{
			Destroy(fx, timeBeforeDestroyFX);
		}
	}

	private void Jump(Vector3 appliedJumpForce)
	{
		if(resetVelocityOnAirJump)
		{
			rigid.velocity = Vector3.zero;
		}
		else
		{
			Vector3 velocity = rigid.velocity;
			velocity.y = 0;
			rigid.velocity = velocity;
		}

		rigid.AddForce(appliedJumpForce, ForceMode.Impulse);
	}

	public IEnumerator CharaJump()
	{
		yield return null;
	}

	private void OnCollisionEnter (Collision collision)
	{
		GroundCheck();
	}

	private void OnDrawGizmosSelected ()
	{
		if (!displayDebugInfo) return;
		
		Vector3 checkPosition = transform.position;
		if (c != null && c.currentGravityForce.y > 0)
		{
			checkPosition += Vector3.up * characterHeight;
		}

		Gizmos.color = Helper.GroundCheck(checkPosition, collisionOffset, transform, collisionCheckRadius, jumpLayerMask) ? Color.blue : Color.red;
		Gizmos.DrawWireSphere(checkPosition + collisionOffset, collisionCheckRadius);
	}
}
