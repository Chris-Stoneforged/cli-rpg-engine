namespace Events.Definitions;

public interface IEventTrigger
{
	void Emit<TEvent>(TEvent request) where TEvent : IEvent;
}