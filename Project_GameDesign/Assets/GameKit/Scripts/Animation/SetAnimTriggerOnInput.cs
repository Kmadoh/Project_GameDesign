using System.Collections.Generic;
using UnityEngine;

public class SetAnimTriggerOnInput : MonoBehaviour
{
	[System.Serializable]
	public class TriggerInfo
	{
		public string triggerParameterName = "Trigger";
		public Animator animator;

		public TriggerInfo(string parameterName, Animator animator)
		{
			this.animator = animator;
			triggerParameterName = parameterName;
		}
	}

	[SerializeField] private bool playOnce = false;
	[SerializeField] private string inputName = "Fire1";
	
	public List<TriggerInfo> triggers = new List<TriggerInfo>(1);

	private bool hasPressed = false;
	[SerializeField] private int inputChoiceIndex;

	private void Start ()
	{
		if(triggers.Count == 0)
		{
			Debug.LogWarning("No Trigger set !", gameObject);
		}
	}

	private void InputCheck()
	{
		if (!Input.GetButtonDown(inputName)) return;
		
		foreach (TriggerInfo t in triggers)
		{
			t.animator.SetTrigger(t.triggerParameterName);
		}
		hasPressed = true;
	}
	
	private void Update ()
	{
		if (playOnce && hasPressed) return;

		InputCheck();
	}
}
