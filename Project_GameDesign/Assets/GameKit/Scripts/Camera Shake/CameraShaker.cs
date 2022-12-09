using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
	public AnimationCurve intensityCurveX = new AnimationCurve(
		new Keyframe(0f, 0f),
		new Keyframe(0.2f, 1f),
		new Keyframe(1.0f, 0f));
	public AnimationCurve intensityCurveY = new AnimationCurve(
		new Keyframe(0f, 0f),
		new Keyframe(0.2f, 1f),
		new Keyframe(1.0f, 0f));
	public AnimationCurve intensityCurveZ = new AnimationCurve(
		new Keyframe(0f, 0f),
		new Keyframe(0.2f, 1f),
		new Keyframe(1.0f, 0f));

	public bool shakeOnXAxis = true;
	public bool shakeOnYAxis = true;
	public bool shakeOnZAxis = false;

	float intensityCurveIndex = 0.0f;
	Vector3 initialPos;
	// Use this for initialization
	void Start ()
	{
		initialPos = transform.localPosition;
	}

	public void Shake (float duration, float intensity)
	{
		if (duration > 0)
		{
			StopAllCoroutines();
			StartCoroutine(DoShake(intensity, duration));
		}
	}

	IEnumerator DoShake (float intensity, float duration)
	{
		intensityCurveIndex = 0f;

		while (intensityCurveIndex < duration)
		{
			intensityCurveIndex += Time.deltaTime;
		
			Vector3 randomPoint = Vector3.zero;
			if (shakeOnXAxis)
			{
				randomPoint.x = Random.Range(-1f, 1f) * intensity * intensityCurveX.Evaluate(intensityCurveIndex / duration);
			}
			if (shakeOnYAxis)
			{
				randomPoint.y = Random.Range(-1f, 1f) * intensity * intensityCurveY.Evaluate(intensityCurveIndex / duration);
			}
			if (shakeOnZAxis)
			{
				randomPoint.z = Random.Range(-1f, 1f) * intensity * intensityCurveZ.Evaluate(intensityCurveIndex / duration);
			}
			transform.localPosition = randomPoint;
			yield return null;
		}

		transform.localPosition = initialPos;
	}
}
