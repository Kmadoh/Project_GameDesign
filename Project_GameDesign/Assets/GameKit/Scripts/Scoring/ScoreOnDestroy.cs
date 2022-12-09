using UnityEngine;

public class ScoreOnDestroy : MonoBehaviour
{
	[Tooltip("How many points we add or substract from the score ?")]
	public int scoreModificationAmount;
	public ScoreManager scoreManager;

	private void Start ()
	{
		if(scoreManager == null)
		{
			scoreManager = FindObjectOfType<ScoreManager>();
		}
	}
	private void OnDestroy ()
	{
		scoreManager.UpdateScore(scoreModificationAmount);
	}
}