namespace Events.Definitions;

public abstract class AEvent : EventArgs, IEvent
{
	public bool IsConsumed { get; private set; }

	public void Consume()
	{
		IsConsumed = true;
	}
}