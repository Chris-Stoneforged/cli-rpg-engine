using Debug;
using Events.Definitions;

namespace Events;

public class EventManager : IEventListener, IEventDispatcher
{
	private interface IEventListener { }

	private class EventListener<TEvent>(
		Action<TEvent> callback
	) : IEventListener where TEvent : IEvent
	{
		public Action<TEvent> Callback { get; } = callback;
	}

	private readonly Dictionary<Type, List<IEventListener>> _eventListeners = [];

	public void DispatchEvent<TEvent>(TEvent @event) where TEvent : IEvent
	{
		if (!_eventListeners.TryGetValue(typeof(TEvent), out var listeners))
		{
			DebugLog.Warn($"No event listeners of type {typeof(TEvent).Name}");
			return;
		}

		DebugLog.Info($"Dispatching event {@event}");
		foreach (var listener in listeners)
		{
			if (listener is not EventListener<TEvent> typedListener) continue;
			typedListener.Callback.Invoke(@event);
		}
	}

	public void RegisterListener<TEvent>(Action<TEvent> handler) where TEvent : IEvent
	{
		if (!_eventListeners.TryGetValue(typeof(TEvent), out var handlers))
		{
			handlers = [];
			_eventListeners.Add(typeof(TEvent), handlers);
		}

		handlers.Add(new EventListener<TEvent>(handler));
	}
}