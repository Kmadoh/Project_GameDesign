using UnityEngine;

public class PlaySound : MonoBehaviour
{
	[SerializeField] private AudioClip soundToPlay;
	[SerializeField] private bool playOnInput;
	[SerializeField] private string inputName;
	[SerializeField] private int inputChoiceIndex;

	private void Start ()
	{
		if(!playOnInput)
		{
			AudioSource.PlayClipAtPoint(soundToPlay, transform.position);
		}
	}
	
	private void Update ()
	{
		if (!playOnInput) return;
		
		if(Input.GetButtonDown(inputName))
		{
			AudioSource.PlayClipAtPoint(soundToPlay, transform.position);
		}
	}
}
