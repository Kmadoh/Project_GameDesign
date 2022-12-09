using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleChangeOnTrigger : MonoBehaviour
{
    public bool useTag = true;
    public string tagName = "Player";

    [SerializeField] private float targetTimeScale = 0.2f;
    [SerializeField] private float timeToReach = 0.2f;
    [SerializeField] private AnimationCurve timeScaleCurve = AnimationCurve.Linear(0,0,1,1);

    [SerializeField] private bool resetTimeScaleOnLeave = true;
    [SerializeField] private bool resetTimeScaleOnDestroy = true;

    private float timer = 0f;

	private void OnTriggerEnter (Collider other)
    {
        if (useTag && !other.CompareTag(tagName)) return;

        TriggerTimeSlow();
    }

	private void OnTriggerExit (Collider other)
    {
        if (!resetTimeScaleOnLeave) return;

        if (useTag && !other.CompareTag(tagName)) return;

        RevertTimeSlow();
    }

	private void OnDestroy ()
    {
        if (!resetTimeScaleOnDestroy) return;
        
        Time.timeScale = 1f;
        Time.fixedDeltaTime = .02f;
    }


    private  IEnumerator ChangeTimeSpeed (float targetSpeed)
    {
        timer = 0f;
        float baseTimeScale = Time.timeScale;
        float baseFixedTimeScale = Time.fixedDeltaTime;
        while (timer < timeToReach)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / timeToReach;
            
            Time.timeScale = Helper.CurvedLerp(baseTimeScale, targetSpeed, timeScaleCurve, t);
            Time.fixedDeltaTime = Helper.CurvedLerp(baseFixedTimeScale, targetSpeed * 0.02f, timeScaleCurve, t);
            yield return null;
        }
    }

    private void TriggerTimeSlow ()
    {
        if (timeToReach > 0f)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeTimeSpeed(targetTimeScale));
        }
        else
        {
            Time.timeScale = targetTimeScale;
            Time.fixedDeltaTime = targetTimeScale * 0.02f;
        }
    }

    private void RevertTimeSlow ()
    {
        if (timeToReach > 0f)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeTimeSpeed(1f));
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = .02f;
        }
    }

}
