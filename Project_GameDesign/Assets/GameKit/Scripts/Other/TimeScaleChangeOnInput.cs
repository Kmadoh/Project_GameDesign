using System;
using System.Collections;
using UnityEngine;

public class TimeScaleChangeOnInput : MonoBehaviour
{
	public enum RevertMode { Toggle, Release, Timer };
    public RevertMode revertMode = RevertMode.Toggle;

    public string inputName = "Fire1";

    [SerializeField] private float targetTimeScale = 0.2f;
    [SerializeField] private float timeToReach = 0.2f;
    [SerializeField] private AnimationCurve timeScaleCurve = AnimationCurve.Linear(0,0,1,1);

    [SerializeField] private float revertTimerDuration = 2f;

    private bool hasBeenPressed = false;

    private float timer = 0f;
    public int inputChoiceIndex;

    private void Update()
    {
        if (Input.GetButtonDown(inputName))
		{
			switch (revertMode)
			{
				case RevertMode.Toggle:
					{
                        if (hasBeenPressed)
                        {
                            RevertTimeSlow();
                        }
                        else
						{
                            TriggerTimeSlow();
						}

                        hasBeenPressed = !hasBeenPressed;
					}
                
				break;
				case RevertMode.Release:
					{
                        TriggerTimeSlow();
					}
				break;
				case RevertMode.Timer:
					{
                        TriggerTimeSlow();
                        StartCoroutine(RevertTimeScaleAfterSeconds());
					}
				break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

        if (!Input.GetButtonUp(inputName)) return;
        
        if(revertMode == RevertMode.Release)
        {
	        RevertTimeSlow();
        }
    }

    private IEnumerator RevertTimeScaleAfterSeconds()
	{
        yield return new WaitForSecondsRealtime(revertTimerDuration);

        RevertTimeSlow();
	}

    private IEnumerator ChangeTimeSpeed (float targetSpeed)
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

	private void OnDestroy ()
	{
        Time.timeScale = 1f;
        Time.fixedDeltaTime = .02f;
    }
}
