namespace Events.Definitions;

public interface IEventRegister
{
	public void RegisterListener<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
}