using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeOnTrigger : MonoBehaviour
{
	[Range(0f, 3f)]
	[SerializeField] float shakeDuration = 0.5f;
	[Range(0f, 3f)]
	[SerializeField] float intensity = 0.5f;

	[SerializeField] CameraShaker targetToShake;

	public bool useTag = true;
	public string tagName = "Player";
	[SerializeField] bool triggerOnce = true;

	bool hasTriggered = false;


	private void Start ()
	{
		if(targetToShake == null)
		{
			targetToShake = FindObjectOfType<CameraShaker>();
		}
	}
	private void OnTriggerEnter (Collider other)
	{
		if(useTag && !other.CompareTag(tagName))
		{
			return;
		}

		if (triggerOnce && hasTriggered)
		{
			return;
		}
		else
		{
			hasTriggered = true;
		}

		targetToShake.Shake(shakeDuration, intensity);	
	}
}