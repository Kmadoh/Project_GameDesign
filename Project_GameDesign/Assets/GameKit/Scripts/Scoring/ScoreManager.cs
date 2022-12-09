using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
	public int score;
	public int initialScore = 0;
	public string scoreTitle = "Score : ";
	public Text scoreText;

	LoadSceneOnScoreReach sceneLoader;

	private void Awake ()
	{
		score = initialScore;
		sceneLoader = FindObjectOfType<LoadSceneOnScoreReach>();
	}
	private void Start ()
	{
		if(scoreText == null)
		{
			scoreText = GetComponentInChildren<Text>();
		}
		scoreText.text = scoreTitle + score;
	}
	public void UpdateScore(int scoreModif)
	{
		score += scoreModif;
		if(score < 0)
		{
			score = 0;
		}

		if(scoreText)
		{
			scoreText.text = scoreTitle + score;
		}

		if(sceneLoader)
		{
			sceneLoader.CompareScore(score);
		}

	}

}
