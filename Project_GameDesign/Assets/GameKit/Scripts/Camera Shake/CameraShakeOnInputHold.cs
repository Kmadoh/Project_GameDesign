using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeOnInputHold : MonoBehaviour
{
	[SerializeField] float intensity = 0.5f;
	[SerializeField] float frequency = 0.05f;
	[SerializeField] float smoothTime = 0.05f;
	float timer = 0f;

	[SerializeField] Transform shakerTransform;

	public string inputName = "Fire1";

	Vector3 targetPos;
	Vector3 velocity;
	Vector3 initialPos;
	public int inputChoiceIndex;

	private void Awake ()
	{
		initialPos = shakerTransform.localPosition;
	}

	private void Update ()
	{
		if(Input.GetButton(inputName))
		{
			CamShake();
			shakerTransform.localPosition = Vector3.SmoothDamp(shakerTransform.localPosition, targetPos, ref velocity, smoothTime);
		}
		else if(Input.GetButtonUp(inputName))
		{
			shakerTransform.localPosition = initialPos;
		}
	}
	public void CamShake ()
	{
		if (timer >= frequency)
		{
			Vector3 randomPoint = initialPos + Random.insideUnitSphere * intensity;

			targetPos = randomPoint;
			timer = 0f;
		}
		else
		{
			timer += Time.unscaledDeltaTime;
		}
	}
}
