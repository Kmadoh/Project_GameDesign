using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsOnInput : MonoBehaviour
{
	public string inputName;
	public bool disableInstead;
	public bool displayDebugInfo;
	public List<GameObject> gameObjectsToEnable = new List<GameObject>();

	public enum RevertOn {InputUp, SecondPress, Timer, Never };

	public RevertOn revertOn = RevertOn.SecondPress;

	public float revertAfterCooldown = 1f;

	private bool wasActivated = false;
	public int inputChoiceIndex;

	private void EnableComponents (bool enable)
	{
		if (wasActivated == !enable)
		{
			if (disableInstead)
			{
				foreach (GameObject t in gameObjectsToEnable)
				{
					t.SetActive(!enable);
				}
			}
			else
			{
				foreach (GameObject t in gameObjectsToEnable)
				{
					t.SetActive(enable);
				}
			}
		}
		wasActivated = enable;
	}

	private IEnumerator RevertAfterTime ()
	{
		yield return new WaitForSeconds(revertAfterCooldown);

		EnableComponents(false);
	}

	private void Update ()
	{
		if (Input.GetButtonDown(inputName))
		{
			if(revertOn == RevertOn.SecondPress)
			{
				EnableComponents(!wasActivated);
			}
			else
			{
				EnableComponents(true);

				if(revertOn == RevertOn.Timer)
				{
					StopAllCoroutines();
					StartCoroutine(RevertAfterTime());
				}
			}
			
		}

		if (revertOn != RevertOn.InputUp) return;
		
		if (Input.GetButtonUp(inputName))
		{
			EnableComponents(false);
		}
	}
}
