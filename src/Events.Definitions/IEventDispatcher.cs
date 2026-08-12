namespace Events.Definitions;

public interface IEventDispatcher
{
	void DispatchEvent<TEvent>(TEvent @event) where TEvent : IEvent;
}