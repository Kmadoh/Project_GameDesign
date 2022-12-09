using UnityEngine;

public class Rotator : MonoBehaviour
{
	[Tooltip("Rotation speed on all 3 axis")]
	public Vector3 angularVelocity;
	[Tooltip("Do we need to press any input to rotate the object ?")]
	public bool useInput;
	[Tooltip("Name of the input used for rotating")]
	public string inputName;

	public int inputChoiceIndex;

	private void RotateOnInput()
	{
		float angSpd = Input.GetAxis(inputName);
		if(angSpd != 0)
		{
			transform.Rotate(angularVelocity * angSpd * Time.deltaTime);
		}
	}

	private void Rotate()
	{
		transform.Rotate(angularVelocity * Time.deltaTime);
	}

	private void Update ()
	{
		if(useInput)
		{
			RotateOnInput();
		}
		else
		{
			Rotate();
		}
	}
}