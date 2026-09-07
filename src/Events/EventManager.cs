using Debug;
using Events.Definitions;

namespace Events;

public class EventManager : IEventTrigger, IEventHandler
{
	private class EventHandler<TEvent>(
		Action<TEvent> callback,
		int priority
	)
	{
		public Action<TEvent> Callback { get; } = callback;
		public int Priority { get; } = priority;
	}

	private readonly Dictionary<Type, List<object>> _requestHandlers = [];

	public void Emit<TEvent>(TEvent @event) where TEvent : IEvent
	{
		if (!_requestHandlers.TryGetValue(typeof(TEvent), out var handlers))
		{
			DebugLog.Warn($"No request handers for type {typeof(TEvent).Name}");
			return;
		}

		DebugLog.Info($"Making request {@event}");
		foreach (var handler in handlers)
		{
			if (@event.IsConsumed) return;
			if (handler is not EventHandler<TEvent> typedHandler) continue;
			typedHandler.Callback.Invoke(@event);
		}
	}

	public void Register<TEvent>(
		Action<TEvent> handler,
		int priority = 0
	) where TEvent : IEvent
	{
		if (!_requestHandlers.TryGetValue(typeof(TEvent), out var handlers))
		{
			handlers = [];
			_requestHandlers.Add(typeof(TEvent), handlers);
		}

		// TODO: Insert in priority order
		handlers.Add(new EventHandler<TEvent>(handler, priority));
	}

	public void Unregister<TEvent>(Action<TEvent> handler) where TEvent : IEvent
	{
		// TODO: this
	}
}