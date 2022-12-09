using System.Collections.Generic;
using UnityEngine;

public class SetAnimBoolOnInput : MonoBehaviour
{
	[System.Serializable]
	public class BoolInfo
	{
		public string boolParameterName;
		public bool valueSetOnInput = true;
		public Animator animator;

		public BoolInfo (string boolParameterName, bool valueSetOnInput, Animator animator)
		{
			this.boolParameterName = boolParameterName;
			this.valueSetOnInput = valueSetOnInput;
			this.animator = animator;
		}
	}

	private  enum RevertOn { InputUp, SecondPress, Never };
	[SerializeField] private RevertOn revertOn = RevertOn.SecondPress;

	[SerializeField] private string inputName = "Fire1";

	public List<BoolInfo> bools = new List<BoolInfo>(1);

	private bool wasActivated = false;
	[SerializeField] private int inputChoiceIndex;

	private void Start ()
	{
		if (bools.Count == 0)
		{
			Debug.LogWarning("No Bools set !", gameObject);
		}
	}

	private void SetBool (bool toggle)
	{
		foreach (BoolInfo t in bools)
		{
			if (t.valueSetOnInput)
			{
				t.animator.SetBool(t.boolParameterName, toggle);
			}
			else
			{
				t.animator.SetBool(t.boolParameterName, !toggle);
			}
		}

		wasActivated = toggle;
	}

	private void Update ()
	{
		if (Input.GetButtonDown(inputName))
		{
			if (revertOn == RevertOn.SecondPress)
			{
				SetBool(!wasActivated);
			}
			else
			{
				SetBool(true);
			}

		}

		if (revertOn != RevertOn.InputUp) return;
		
		if (Input.GetButtonUp(inputName))
		{
			SetBool(false);
		}
	}
}
