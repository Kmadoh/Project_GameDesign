using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
	[SerializeField] private Transform transformToRotateWithCamera;

	public string mouseXInputName = "Mouse X";
	public string mouseYInputName = "Mouse Y";
	
	[Range(50f, 500f)]
	[SerializeField] private float mouseSensitivityX = 250f;
	
	[Range(50f, 500f)]
	[SerializeField] private float mouseSensitivityY = 250f;
	
	[SerializeField] public float minXAxisClamp = -85f;
	[SerializeField] public float maxXAxisClamp = 85f;
	private float xAxisClamp;

	[SerializeField] private bool displayDebugInfo;
	
	[SerializeField] private int xInputChoiceIndex;
	[SerializeField] private int yInputChoiceIndex;
	
	private void Awake ()
	{
		LockCursor();
		xAxisClamp = 0;

		if (transformToRotateWithCamera != null) return;
		
		Debug.LogWarning("No Player Body Set !", gameObject);
		transformToRotateWithCamera = GetComponentInParent<Mover>().transform;
	}

	private static void LockCursor ()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void CameraRotation ()
	{
		float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivityX * Time.deltaTime;
		float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivityY * Time.deltaTime;

		xAxisClamp += mouseY;

		if(xAxisClamp > maxXAxisClamp)
		{
			xAxisClamp = maxXAxisClamp;
			mouseY = 0.0f;
			ClampXAxisRotationToValue(360f - maxXAxisClamp);
		}
		if (xAxisClamp < minXAxisClamp)
		{
			xAxisClamp = minXAxisClamp;
			mouseY = 0.0f;
			ClampXAxisRotationToValue(minXAxisClamp * -1f);
		}

		transform.Rotate(Vector3.left * mouseY);
		
		if(transformToRotateWithCamera != null)
		{
			transformToRotateWithCamera.Rotate(Vector3.up * mouseX);	
		}
	}

	private void ClampXAxisRotationToValue(float value)
	{
		Transform transform1 = transform;
		
		Vector3 eulerRotation = transform1.eulerAngles;
		eulerRotation.x = value;
		transform1.eulerAngles = eulerRotation;
	}

	private void FixedUpdate ()
	{
		CameraRotation();
	}
}