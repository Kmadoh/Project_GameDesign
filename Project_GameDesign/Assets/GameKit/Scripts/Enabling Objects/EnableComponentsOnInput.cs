using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnableComponentsOnInput : MonoBehaviour
{
	public string inputName;
	public bool disableInstead;
	public List<Behaviour> components = new List<Behaviour>();

	public enum RevertOn { InputUp, SecondPress, Timer, Never };
	public RevertOn revertOn = RevertOn.SecondPress;

	public float revertAfterCooldown = 1f;
	public bool displayDebugInfo = false;

	private bool wasActivated = false;
	public int inputChoiceIndex;

	private void EnableComponents (bool enable)
	{
		if (wasActivated == !enable)
		{
			if (disableInstead)
			{
				foreach (Behaviour t in components)
				{
					t.enabled = !enable;
				}
			}
			else
			{
				foreach (Behaviour t in components)
				{
					t.enabled = enable;
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
			if (revertOn == RevertOn.SecondPress)
			{
				EnableComponents(!wasActivated);
			}
			else
			{
				EnableComponents(true);

				if (revertOn == RevertOn.Timer)
				{
					StopAllCoroutines();
					StartCoroutine(RevertAfterTime());
				}
			}
		}

		if (revertOn == RevertOn.InputUp && Input.GetButtonUp(inputName))
		{
			EnableComponents(false);
		}
	}
}
