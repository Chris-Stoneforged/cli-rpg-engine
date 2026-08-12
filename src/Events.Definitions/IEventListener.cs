namespace Events.Definitions;

public interface IEventListener
{
	public void RegisterListener<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
}