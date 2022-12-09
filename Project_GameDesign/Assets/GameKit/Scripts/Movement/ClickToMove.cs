using UnityEngine;

public class ClickToMove : MonoBehaviour
{
	[Tooltip("Camera Used to register clicks from")]
	[SerializeField] private Camera cam;

	[Tooltip("On which axis do we lock the cursor position ? Usually we use Y axis")]
	[SerializeField] private bool lockOnX = false;
	[SerializeField] private bool lockOnY = true;
	[SerializeField] private bool lockOnZ = false;

	[Tooltip("Movement speed of the object towards click")]
	[SerializeField] private float moveSpeed = 5f;
	[Tooltip("Does the object look towards the destination ?")]
	[SerializeField] private bool lookTowardsDestination = true;

	[Tooltip("Offset from click point (Useful if the objects looks below or above)")]
	[SerializeField] private Vector3 offset = new Vector3(0f, 0.2f, 0f);
	[Tooltip("Layer Mask used for raycasting towards the ground")]
	[SerializeField] private LayerMask clickMask = ~0;
	[Tooltip("Game Object reference in scene that serves as click marker")]
	[SerializeField] private GameObject clickMarker = null;

	[Tooltip("Animator reference for movement")]
	[SerializeField] private Animator animator;
	[Tooltip("Bool Parameter name used when moving")]
	[SerializeField] private string isMovingParameterName = "Moving";
	
	[Tooltip("Do we track facing direction ?")]
	[SerializeField] private bool trackFacingDirection = false;
	
	[Tooltip("Float parameter used for horizontal axis")]
	[SerializeField] private string hParameterName = "Horizontal";
	[Tooltip("Float parameter used for vertical axis")]
	[SerializeField] private string vParameterName = "Vertical";
	
	private GameObject currentClickMarker;
	private ParticleSystem particles;

	private Vector3 targetPos;
	private bool isMoving = false;
	private bool isStopped = true;

	public bool displayDebugInfo;
	
	private void Start ()
	{
		if(clickMarker != null)
		{
			currentClickMarker = Instantiate(clickMarker, transform.position, clickMarker.transform.rotation);
			particles = currentClickMarker.GetComponent<ParticleSystem>();

			if(particles == null)
			{
				currentClickMarker.SetActive(false);
			}
			else
			{
				particles.Stop();
			}
		}
		Debug.Log(clickMask);

		if(cam == null)
		{
			cam = Camera.main;
		}

		if (animator != null) return;
		
		animator = GetComponentInChildren<Animator>();
		Debug.LogWarning("No animator referenced !", gameObject);
	}

	private void UpdateAnimValues()
	{
		if (animator == null) return;
		
		animator.SetBool(isMovingParameterName, isMoving);

		if (!trackFacingDirection) return;

		var forward = transform.forward;
		animator.SetFloat(hParameterName, forward.x);
		animator.SetFloat(vParameterName, forward.y);
	}

	private void SetTargetPos()
	{
		if (cam == null) return;

		if (!Input.GetMouseButton(0)) return;
		
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
				
		if (!Physics.Raycast(ray, out hit, 100f, clickMask)) return;
				
		Vector3 cursorPos = hit.point;
		if (lockOnY)
		{
			cursorPos.y = transform.position.y;
		}
		if (lockOnX)
		{
			cursorPos.x = transform.position.x;
		}
		if (lockOnZ)
		{
			cursorPos.z = transform.position.z;
		}

		if (!(Vector3.Distance(cursorPos, transform.position) > 1f)) return;
				
		if(currentClickMarker != null)
		{
			currentClickMarker.transform.position = cursorPos + offset;
			if (particles == null)
			{
				currentClickMarker.SetActive(false);
				currentClickMarker.SetActive(true);
			}
			else
			{
				if(isStopped == true)
				{
					particles.Play();
					isStopped = false;
				}
			}
		}

		targetPos = cursorPos;
		isMoving = true;
	}

	private void MoveTowardsTargetPos()
	{
		if(Vector3.Distance(transform.position, targetPos) > 0.1f)
		{
			Vector3 movement = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
			transform.position = movement;

			if (!lookTowardsDestination) return;
			
			Vector3 cursorPos = targetPos;
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

			transform.LookAt(cursorPos);
		}
		else
		{
			isMoving = false;
			
			if (currentClickMarker == null) return;
			
			if (particles == null)
			{
				currentClickMarker.SetActive(false);
			}
			else
			{
				particles.Stop();
				isStopped = true;
			}
		}

	}
	
	private void Update ()
	{
		SetTargetPos();
		UpdateAnimValues();
	}

	private void FixedUpdate ()
	{
		if (isMoving)
		{
			MoveTowardsTargetPos();
		}
	}
}
