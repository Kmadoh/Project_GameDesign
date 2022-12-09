public class EventsOnDestroy : EventBase
{
	private void OnDestroy ()
	{
		TriggerEvents();
	}
}
