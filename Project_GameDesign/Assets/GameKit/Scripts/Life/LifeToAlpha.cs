using UnityEngine;
using UnityEngine.UI;

public class LifeToAlpha : MonoBehaviour
{
	[SerializeField] private Life lifeToTrack;
	[SerializeField] private Image imageToTweak;
	[SerializeField] private bool invert = false;

	private Color col;

	private void Awake ()
	{
		if(lifeToTrack == null)
		{
			Debug.LogWarning("No Life component referenced !", gameObject);
			lifeToTrack = FindObjectOfType<Life>();
		}
		if(imageToTweak == null)
		{
			Debug.LogWarning("No Image component referenced !", gameObject);
			if (!TryGetComponent<Image>(out imageToTweak))
			{
				imageToTweak = GetComponentInChildren<Image>();
			}
		}

		col = imageToTweak.color;
		
		float baseRatio =  (float)lifeToTrack.CurrentLife / lifeToTrack.maxLife;

		float ratio = invert ? baseRatio : 1f - baseRatio;

		col.a = 1f - ratio;
		imageToTweak.color = col;

		lifeToTrack.lifeChangeDelegate += UpdateAlpha;
		
		UpdateAlpha();
	}

	private void UpdateAlpha ()
	{
		if (lifeToTrack == null) return;
		
		col = imageToTweak.color;

		float baseRatio =  (float)lifeToTrack.CurrentLife / lifeToTrack.maxLife;

		float ratio = invert ? baseRatio : 1f - baseRatio;


		col.a = Mathf.MoveTowards(col.a, 1f - ratio, 0.3f * Time.deltaTime);
		//Debug.Log("Ratio : " + ratio);

		imageToTweak.color = col;
	}
}
