using UnityEngine;

public class EventsOnCollide : EventBase
{
    [Tooltip("Do we check for tags on trigger enter ?")]
    public bool useTag = false;
    [Tooltip("Tag name used on trigger enter ?")]
    public string tagName = "Player";

	/// <summary>
	/// Checks if colliding tag is the same as the one we require
	/// </summary>
	/// <param name="other">Reference to colliding object</param>
	/// <returns>Returns true if tags match, or if usetag is set to false</returns>
    private bool TagCheck(GameObject other)
	{
		return !useTag || other.CompareTag(tagName);
	}

	private void OnCollisionEnter (Collision other)
	{
		if(TagCheck(other.gameObject))
		{
			TriggerEvents();
		}
	}
}
