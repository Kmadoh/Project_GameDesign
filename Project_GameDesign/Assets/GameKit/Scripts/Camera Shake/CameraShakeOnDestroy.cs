using UnityEngine;

public class CameraShakeOnDestroy : MonoBehaviour
{
	[Range(0f, 3f)]
	[SerializeField] private float shakeDuration = 1f;
	[Range(0f, 3f)]
	[SerializeField] private float intensity = 1f;

	[SerializeField] private CameraShaker targetToShake;

	[SerializeField] private SceneHandler sceneHandler;

	private void Awake ()
	{
		if(targetToShake == null)
		{
			targetToShake = FindObjectOfType<CameraShaker>();
		}

		if(sceneHandler == null)
		{
			sceneHandler = FindObjectOfType<SceneHandler>();
		}
	}

	private void OnDestroy ()
	{
		if (SceneHandler.isLoadingScene)
		{
			return;
		}

		targetToShake.Shake(shakeDuration, intensity);
	}
}
