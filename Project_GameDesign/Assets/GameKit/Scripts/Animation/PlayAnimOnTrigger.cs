using UnityEngine;

public class PlayAnimOnTrigger : MonoBehaviour
{
	[SerializeField] private Animator animator = null;
	[SerializeField] private string triggerName = "Case Sensitive";
	[SerializeField] private bool useTag = false;
	[SerializeField] private string tagName = "Case Sensitive";
	[SerializeField] private bool triggerOnce = true;

	private bool hasPlayed = false;

	private void OnTriggerEnter (Collider other)
	{
		if (triggerOnce && hasPlayed) return;
		
		if (useTag && !other.CompareTag(tagName)) return;

		if (animator != null)
		{
			hasPlayed = true;
			animator.SetTrigger(triggerName);
		}
		else
		{
			Debug.LogWarning("No animator set !", gameObject);
		}
	}
}
