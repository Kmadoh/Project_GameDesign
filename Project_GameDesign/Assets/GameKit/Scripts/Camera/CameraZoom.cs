using UnityEngine;

	[RequireComponent(typeof(Camera))]
	public class CameraZoom : MonoBehaviour
	{
		[SerializeField] private AnimationCurve zoomCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		public float minZoomFOV = 15f;
		public float maxZoomFOV = 60f;

		[SerializeField] private float sensitivity = 50f;
		[SerializeField] private float smoothSpeed = 2f;

		public string zoomInputName = "Mouse ScrollWheel";
		public Camera usedCamera;

		public float currentZoom;
		private float t = 0;
		public int inputChoiceIndex;

		private void Start ()
		{
			if(usedCamera == null)
			{
				Debug.LogWarning("No camera set !", gameObject);
				usedCamera = UnityEngine.Camera.main;
			}
		
			if(usedCamera != null) currentZoom = usedCamera.fieldOfView;
		}
	
		private void Update ()
		{
			float zoomAxis = Input.GetAxis(zoomInputName);
			if (zoomAxis != 0f)
			{
				t = Mathf.Clamp(t + Time.deltaTime * zoomAxis * sensitivity, 0f, 1f);
				currentZoom = Helper.CurvedLerp(minZoomFOV, maxZoomFOV, zoomCurve, 1 - t);
			}
			usedCamera.fieldOfView = Mathf.MoveTowards(usedCamera.fieldOfView, currentZoom, smoothSpeed);
		}
	}