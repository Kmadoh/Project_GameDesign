using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsOnLifeValue : MonoBehaviour
{
	public enum ValueTrigger
	{
		Lower,
		Equal,
		Greater
	};
	
	public ValueTrigger valueTrigger = ValueTrigger.Lower;
	public bool disableInstead = false;
	public bool displayDebugInfo = false;

	[SerializeField] private Life life = null;

	public List<GameObject> gameObjectsToEnable = new List<GameObject>();

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

	private void UpdateComponents(bool state)
	{
		foreach (GameObject b in gameObjectsToEnable)
		{
			b.SetActive(state);
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
		}
	}
}
