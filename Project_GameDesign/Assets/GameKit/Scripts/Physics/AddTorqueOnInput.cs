using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AddTorqueOnInput : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;

	[SerializeField] private ForceMode forceMode = ForceMode.Force;
	[SerializeField] private bool isTorqueLocal = true;
    [SerializeField] private Vector3 torque = Vector3.zero;

    public string inputName = "Horizontal";
    public int inputChoiceIndex;

    private void Awake ()
	{
		if(rigid == null)
		{
			rigid = GetComponent<Rigidbody>();
		}
	}

	private void FixedUpdate()
	{
		float axisValue = Input.GetAxis(inputName);
		if ( axisValue == 0f) return;
		
		if(isTorqueLocal)
		{
			rigid.AddRelativeTorque(torque * axisValue, forceMode);
		}
		else
		{
			rigid.AddTorque(torque * axisValue, forceMode);
		}
	}
}
