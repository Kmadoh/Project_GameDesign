using System;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentsOnLifeValue : MonoBehaviour
{
	public enum ValueTrigger
	{
		Lower,
		Equal,
		Greater
	};
	
	public ValueTrigger valueTrigger = ValueTrigger.Lower;
	[SerializeField] private bool disableInstead = false;
	[SerializeField] private bool displayDebugInfo;

	[SerializeField] private Life life = null;

	public List<Behaviour> components = new List<Behaviour>();

	[SerializeField] private int lifeReachedToTrigger = 0;
	[SerializeField] private bool triggerOnce = true;

	private bool hasTriggered = false;

	private void Start ()
	{
		if (life != null)
		{
			life.lifeChangeDelegate += OnLifeChange;
			return;
		}
		
		life = FindObjectOfType<Life>();
		life.lifeChangeDelegate += OnLifeChange;
		Debug.LogWarning("No Life component set ! Finding one by default in the scene", gameObject);
	}

	void UpdateComponents(bool state)
	{
		foreach (Behaviour b in components)
		{
			b.enabled = state;
		}
	}

	private void OnLifeChange ()
	{
		if (triggerOnce && hasTriggered)
		{
			return;
		}

		switch (valueTrigger)
		{
			case ValueTrigger.Lower:
			{
				if (life.CurrentLife < lifeReachedToTrigger)
				{
					UpdateComponents(!disableInstead);
					hasTriggered = true;
				}
			} 
			break;
			case ValueTrigger.Equal:
			{
				if (life.CurrentLife == lifeReachedToTrigger)
				{
					UpdateComponents(!disableInstead);
					hasTriggered = true;
				}
			}
			break;
			case ValueTrigger.Greater:
			{
				if (life.CurrentLife > lifeReachedToTrigger)
				{
					UpdateComponents(!disableInstead);
					hasTriggered = true;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}
