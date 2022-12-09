using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsOnTrigger : EventBase
{
    [Tooltip("Used if you want to launch the next event when an object leaves trigger area")]
    [SerializeField] private bool progressOnTriggerExit = false;
    [Tooltip("Do we check for tags on trigger enter ?")]
    public bool useTag = false;
    [Tooltip("Tag name used on trigger enter ?")]
    public string tagName = "Player";

	/// <summary>
	/// Checks if colliding tag is the same as the one we require
	/// </summary>
	/// <param name="other">Reference to colliding object</param>
	/// <returns>Returns true if tags match, or if usetag is set to false</returns>
    private bool TagCheck(Collider other)
	{
		return !useTag || other.CompareTag(tagName);
	}

	private void OnTriggerEnter (Collider other)
	{
		if(TagCheck(other))
		{
			TriggerEvents();
		}
	}

	private void OnTriggerExit (Collider other)
	{
		if(!progressOnTriggerExit)
		{
			return;
		}

		if (TagCheck(other))
		{
			TriggerEvents();
		}
	}
}
