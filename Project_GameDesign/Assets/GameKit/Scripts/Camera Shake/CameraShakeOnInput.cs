using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeOnInput : MonoBehaviour
{
	[Range(0f, 3f)]
	[SerializeField] private float shakeDuration = 1f;
	[Range(0f, 3f)]
	[SerializeField] private float intensity = 0.5f;

	public string inputName = "Fire1";

	public bool useCooldown = true;
	[SerializeField] private float cooldown = 1.0f;

	[SerializeField] private bool onlyOnce = false;

	[SerializeField] private CameraShaker targetToShake = null;

	private bool hasShaken = false;

	private float timer;
	public int inputChoiceIndex;

	private void Start ()
	{
		if (targetToShake == null)
		{
			targetToShake = FindObjectOfType<CameraShaker>();
		}
	}

	private bool CanShake()
	{
		if(timer > 0f)
		{
			timer -= Time.deltaTime;
			return false;
		}
		else
		{
			return true;
		}
	}
	
	private void Update ()
	{
		if(useCooldown)
		{
			if(!CanShake())
			{
				return;
			}

			if (onlyOnce && hasShaken) return;

			if (!Input.GetButton(inputName)) return;
			
			targetToShake.Shake(shakeDuration, intensity);
			timer = cooldown;
			hasShaken = true;
		}
		else
		{
			if (onlyOnce && hasShaken) return;

			if (!Input.GetButtonDown(inputName)) return;
			
			hasShaken = true;
			targetToShake.Shake(shakeDuration, intensity);
		}
	}
}
