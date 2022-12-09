using UnityEngine;

public class EventsOnInput : EventBase
{
    [Tooltip("Input used for triggering the events")]
    public string inputName = "Input Name (Case Sensitive)";

    public int inputChoiceIndex;

    public override void Update()
    {
        base.Update();

        if(Input.GetButtonDown(inputName))
		{
            TriggerEvents();
        }
    }
}
