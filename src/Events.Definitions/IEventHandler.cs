namespace Events.Definitions;

public interface IEventHandler
{
	public void Register<TEvent>(
		Action<TEvent> handler,
		int priority = 0
	) where TEvent : IEvent;

	public void Unregister<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
}