using System;
using UnityEngine;

public class LoadSceneOnScoreReach : MonoBehaviour
{
	private enum Compare { StrictlySuperior, StrictlyInferior, StrictlyEqual, SuperiorOrEqual, InferiorOrEqual}

	[SerializeField] private ScoreManager scoreMgr;

	[SerializeField] private int scoreToReach = 0;

	[SerializeField] private Compare compareCheck = Compare.SuperiorOrEqual;

	public string sceneToLoad = "Scene Name In Assets (Case Sensitive)";
	public int sceneChoiceIndex;

	private void Start ()
	{
		if (scoreMgr != null) return;
		
		if (!TryGetComponent<ScoreManager>(out scoreMgr))
		{
			scoreMgr = FindObjectOfType<ScoreManager>();
		}
	}

	private void Update ()
	{
		if(scoreMgr)
		{
			CompareScore(scoreMgr.score);
		}
	}

	public void CompareScore(int currentScore)
	{
		switch (compareCheck)
		{
			case Compare.SuperiorOrEqual:

			if(currentScore >= scoreToReach)
			{
				SceneLoading();
			}

			break;

			case Compare.InferiorOrEqual:
			if (currentScore <= scoreToReach)
			{
				SceneLoading();
			}
			break;

			case Compare.StrictlyEqual:

			if (currentScore == scoreToReach)
			{
				SceneLoading();
			}

			break;

			case Compare.StrictlyInferior:

			if (currentScore < scoreToReach)
			{
				SceneLoading();
			}

			break;

			case Compare.StrictlySuperior:

			if (currentScore > scoreToReach)
			{
				SceneLoading();
			}

			break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void SceneLoading()
	{
		SceneHandler sceneManager = FindObjectOfType<SceneHandler>();

		if(sceneManager != null)
		{
			sceneManager.LoadScene(sceneToLoad);
		}
		else
		{
			Debug.LogError("No Scene Manager found in the scene ! Please add one from the GameKit Prefabs.", gameObject);
		}
	}
}
