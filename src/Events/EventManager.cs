using Debug;
using Events.Definitions;

namespace Events;

public class EventManager : IEventManager
{
	private interface IEventHandler
	{
		int Priority { get; }
	}

	private class EventHandler<TEvent>(
		Action<TEvent> callback,
		int priority
	) : IEventHandler
	{
		public Action<TEvent> Callback { get; } = callback;
		public int Priority { get; } = priority;
	}

	private readonly Dictionary<Type, List<IEventHandler>> _requestHandlers = [];
	//private readonly Queue<IEvent> _eventQueue = [];

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
		Action<TEvent> callback,
		int priority = 0
	) where TEvent : IEvent
	{
		if (!_requestHandlers.TryGetValue(typeof(TEvent), out var handlers))
		{
			handlers = [];
			_requestHandlers.Add(typeof(TEvent), handlers);
		}

		// TODO: Optimize
		handlers.Add(new EventHandler<TEvent>(callback, priority));
		handlers.Sort((a, b) => a.Priority - b.Priority);
	}

	public void Unregister<TEvent>(Action<TEvent> callback) where TEvent : IEvent
	{
		if (_requestHandlers.TryGetValue(typeof(TEvent), out var handlers))
		{
			var existing = handlers.FirstOrDefault(
				h => h is EventHandler<TEvent> typedHandler && typedHandler.Callback == callback
			);

			if (existing != null)
			{
				handlers.Remove(existing);
			}
		}
	}
}