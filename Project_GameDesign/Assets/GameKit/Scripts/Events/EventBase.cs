using UnityEngine;
using UnityEngine.Events;

public class EventBase : MonoBehaviour
{
    [Tooltip("After reaching the end of the list, do we start over ? If only one UnityEvent is added, can it be triggered at will ??")]
    [SerializeField] bool loop = false;
    public bool displayDebugInfo = false;

    [Tooltip("List of Events triggered, in sequence, after each Input press")]
    [SerializeField] UnityEvent[] triggeredEvents;
    private int index = 0;
    
    /// <summary>
    /// Triggers linked event(s), and increased the index by one
    /// </summary>
    public virtual void TriggerEvents()
	{
        int arrayLength = triggeredEvents.Length;

        if (index >= arrayLength) return;
        
        triggeredEvents[index].Invoke();

        index++;
        
        if (!loop) return;
        
        if(index >= arrayLength)
        {
	        index = 0;
        }
	}

    public virtual void Update ()
	{

	}
}
