using UnityEngine;

public class ScoreOverTime : MonoBehaviour
{
	[Tooltip("Reference to the Score Manager Component")]
	[SerializeField] private ScoreManager scoreManager;

	[Tooltip("How many points we add or substract from the score ?")]
	[SerializeField] private int scoreModificationAmount = 1;

	[Range(0.1f, 10f)]
	[Tooltip("How often do we add or substract from the score ?")]
	[SerializeField] private float scoreModificationFrequency = 1f;

	float timer = 0;

	private void Awake ()
	{
		if(scoreManager == null)
		{
			scoreManager = FindObjectOfType<ScoreManager>();
		}

		if(scoreModificationAmount == 0)
		{
			Debug.LogWarning("Score modif value is equal to 0 !", gameObject);
		}

		timer = scoreModificationFrequency;
	}
	
	private void Update()
    {
		timer -= Time.deltaTime;

		if (!(timer <= 0f)) return;
		if (scoreManager == null) return;
		
		scoreManager.UpdateScore(scoreModificationAmount);
		timer = scoreModificationFrequency;
    }
}
