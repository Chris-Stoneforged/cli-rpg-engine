namespace Events.Definitions;

public interface IEvent
{
	bool IsConsumed { get; }
	void Consume();
}