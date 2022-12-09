using UnityEngine;

public class ScoreOnCollide : MonoBehaviour
{

	[Tooltip("How many points we add or substract from the score ?")]
	public int scoreModificationAmount;
	[Tooltip("Do we change the score only once ?")]
	public bool triggerOnlyOnce;
	[Tooltip("Do we use a tag for collision ?")]
	public bool useTag;
	public string tagName;
	public ScoreManager scoreManager;
	
	private bool hasCollided = false;


	private void Start ()
	{
		if (scoreManager == null)
		{
			scoreManager = FindObjectOfType<ScoreManager>();
		}
	}

	private void OnCollisionEnter (Collision collision)
	{
		if (triggerOnlyOnce && hasCollided) return;

		if (useTag && !collision.gameObject.CompareTag(tagName)) return;
		
		scoreManager.UpdateScore(scoreModificationAmount);
		hasCollided = true;
	}
	
	private void OnTriggerEnter (Collider other)
	{
		if (triggerOnlyOnce && hasCollided) return;

		if (useTag && !other.gameObject.CompareTag(tagName)) return;
		
		scoreManager.UpdateScore(scoreModificationAmount);
		hasCollided = true;
	}
}
