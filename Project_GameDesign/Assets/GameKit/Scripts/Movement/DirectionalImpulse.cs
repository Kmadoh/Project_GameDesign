using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UNCOMMENT ONLY IF USING VFX GRAPH PACKAGE
//using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class DirectionalImpulse : MonoBehaviour
{
	public enum DirectionInputType { Axis, MousePos, Forward }
	public enum Axis { X, Y, Z, None }
	private  enum AxisCheck { X,Y,Z}

	//[Header("Input")]
	public DirectionInputType directionInputType = DirectionInputType.Axis;

	public Axis horizontalAxis = Axis.X;
	public string horizontalAxisName = "Horizontal";

	public Axis verticalAxis = Axis.Y;
	public string verticalAxisName = "Vertical";

	public string impulseInputName = "Fire1";

	[Tooltip("Which axis is the depth ?")]
	[SerializeField] private AxisCheck depthAxis = AxisCheck.Z;

	//[Header("Force")]
	[SerializeField] private bool resetVelocityOnImpulse = false;
	[SerializeField] private float impulseForce = 5f;

	//[Header("Cooldown")]
	public bool useCooldown = false;
	[SerializeField] private float cooldown = 1f;

	//[Header("Collision Check")]
	[Tooltip("Minimal height required to perform an impulse. Helps preventing wall jumps while on the ground.")]
	public float minimalHeightToImpulse = 0;
	[Tooltip("Which axis is the depth ?")]
	[SerializeField] private AxisCheck heightAxis = AxisCheck.Y;
	[SerializeField] private LayerMask groundDetectionLayerMask = 1;
	[Tooltip("Offset from the pivot when detecting collision")]
	[SerializeField] private Vector3 collisionOffset = new Vector3(0, 0.1f, 0);

	//[Header("Resources")]
	public bool requireResources = false;
	[SerializeField] private ResourceManager resourceManager;
	[SerializeField] private string resourceIndex = "";
	[SerializeField] private int resourceCostOnUse = 1;

	//[Header("Move")]
	[Tooltip("Do we prevent moving for some time after impulsion?")]
	public bool preventMovingAfterImpulse = true;
	[Tooltip("Reference to the Mover Component")]
	public Mover mover;
	[Tooltip("Duration of movement prevention")]
	[SerializeField] private float movePreventionDuration = 0.75f;

	//[Header("FX")]
	public Helper.FXFeedbackType fXFeedbackType = Helper.FXFeedbackType.Instantiate;

	[Tooltip("Particle System triggered on Dash (Use World space for particle emission for better results)")]
	public ParticleSystem impulseParticleSystem;

	//UNCOMMENT ONLY IF USING VFX GRAPH PACKAGE
	//[Tooltip("Visual Effect triggered on dash")]
	//public VisualEffect impulseVFX;
	//public string impulseEventName = "Impulse";

	[Tooltip("Visual / Sound FX instantiated on dash")]
	public GameObject spawnedFX;
	[Tooltip("Offset from the pivot when Instantiating FX")]
	[SerializeField] private Vector3 fxOffset = Vector3.zero;
	[Tooltip("Lifetime of the Instantiated FX. 0 = Do not destroy")]
	[SerializeField] private float timeBeforeDestroyFX = 3f;

	[Tooltip("Animator Component")]
	public Animator animator = null;
	[Tooltip("Name of the Trigger Parameter to activate")]
	[SerializeField] private string dashTriggerName = "dash";

	public bool displayDebugInfo = false;

	private float timer = 0f;
	private Rigidbody rigid;
	public int impulseIndex;
	public int horizontalIndex;
	public int verticalIndex;

	private void Awake ()
	{
		rigid = GetComponent<Rigidbody>();

		if(preventMovingAfterImpulse && mover == null)
		{
			mover = GetComponent<Mover>();
		}

		if(useCooldown)
		{
			timer = cooldown;
		}

		if(requireResources && resourceManager == null)
		{
			resourceManager = FindObjectOfType<ResourceManager>();
		}

		if(animator == null)
		{
			animator = GetComponent<Animator>();
		}
	}

	private bool CooldownCheck()
	{
		return !useCooldown || timer.PunctualCooldownCheck(cooldown);
	}

	private bool HeightCheck ()
	{
		if (!(minimalHeightToImpulse > 0f)) return true;

		Vector3 rayDir = heightAxis switch
		{
			AxisCheck.X => Vector3.left,
			AxisCheck.Y => Vector3.down,
			AxisCheck.Z => Vector3.back,
			_ => Vector3.zero
		};

		RaycastHit hit;
			
		return !Physics.Raycast(transform.position + collisionOffset, rayDir, out hit, minimalHeightToImpulse, groundDetectionLayerMask);
	}

	private Vector3 GetMouseDirection ()
	{
		Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (!Physics.Raycast(ray, out hit)) return Vector3.zero;
		
		Vector3 cursorPos = hit.point;
		Vector3 toCursor = cursorPos - transform.position;

		switch (depthAxis)
		{
			case AxisCheck.X:
				toCursor.x = 0;
				break;

			case AxisCheck.Y:
				toCursor.y = 0;
				break;

			case AxisCheck.Z:
				toCursor.z = 0;
				break;
				
			default:
				throw new ArgumentOutOfRangeException();
		}

		toCursor.Normalize();
		return toCursor;

	}

	private Vector3 GetInputDirection ()
	{
		Vector3 inputDir = Vector3.zero;

		switch (horizontalAxis)
		{
			case Axis.X:
			inputDir.x = Input.GetAxis(horizontalAxisName);
			break;

			case Axis.Y:
			inputDir.y = Input.GetAxis(horizontalAxisName);
			break;

			case Axis.Z:
			inputDir.z = Input.GetAxis(horizontalAxisName);
			break;
			case Axis.None:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		switch (verticalAxis)
		{
			case Axis.X:
			inputDir.x = Input.GetAxis(verticalAxisName);
			break;

			case Axis.Y:
			inputDir.y = Input.GetAxis(verticalAxisName);
			break;

			case Axis.Z:
			inputDir.z = Input.GetAxis(verticalAxisName);
			break;
			case Axis.None:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		inputDir.Normalize();

		return inputDir;
	}

	private void ResourceCheck(Vector3 direction)
	{
		if (direction == Vector3.zero) return;

		if (requireResources && !resourceManager.ChangeDicoResourceAmount(resourceIndex, resourceCostOnUse * -1)) return;

		Impulsion(direction);
	}

	void Impulsion (Vector3 direction)
	{
		if(direction != Vector3.zero || directionInputType == DirectionInputType.Forward)
		{
			if (resetVelocityOnImpulse)
			{
				rigid.velocity = Vector3.zero;
			}

			rigid.AddForce(direction * impulseForce, ForceMode.Impulse);

			if(useCooldown)
			{
				timer = cooldown;
			}

			if (preventMovingAfterImpulse)
			{
				if (mover != null)
				{
					StartCoroutine(PreventMovement());
				}
			}

			FXTrigger();

			if (animator != null)
			{
				animator.SetTrigger(dashTriggerName);
			}
		}
	}

	void FXTrigger()
	{
		switch (fXFeedbackType)
		{
			case Helper.FXFeedbackType.Instantiate:
				{
					if (spawnedFX != null)
					{
						GameObject fx = Instantiate(spawnedFX, transform.position + fxOffset, Quaternion.identity);

						if (timeBeforeDestroyFX > 0)
						{
							Destroy(fx, timeBeforeDestroyFX);
						}
					}
				}
			break;

			case Helper.FXFeedbackType.ParticleSystem:
				{ 
					impulseParticleSystem.Play();
				}
			break;

			case Helper.FXFeedbackType.VFXGraph:
				{
					//UNCOMMENT ONLY IF USING VFX GRAPH PACKAGE
					//impulseVFX.SendEvent(impulseEventName);

					//Comment/Delete this line if using VFX Graph Package
					Debug.LogError("If you're using VFX Graph, you might want to uncomment some code lines ! Please edit the script and look for :");
					Debug.LogError("//UNCOMMENT ONLY IF USING VFX GRAPH PACKAGE");
					Debug.LogError("lines and what is below, and remove the //");
				}
			break;
		}
	}

	public IEnumerator PreventMovement()
	{
		mover.canMove = false;

		yield return new WaitForSeconds(movePreventionDuration);

		mover.canMove = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if(CooldownCheck())
		{
			if (Input.GetButtonDown(impulseInputName))
			{
				if (HeightCheck())
				{
					if (directionInputType == DirectionInputType.Axis)
					{

						ResourceCheck(GetInputDirection());
					}
					else if (directionInputType == DirectionInputType.MousePos)
					{
						ResourceCheck(GetMouseDirection());
					}
					else
					{
						ResourceCheck(transform.forward);
					}
				}
			}
		}
	}
}
