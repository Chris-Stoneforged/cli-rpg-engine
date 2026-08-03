using Requests.Definitions;

namespace Requests;

public class RequestRegister : IRequestRegister, IRequestMaker
{
	private interface IRequestHandler { }

	private class RequestHandler<TRequest>(
		Action<TRequest> callback,
		int priority
	) : IRequestHandler where TRequest : IRequest
	{
		public Action<TRequest> Callback { get; } = callback;
		public int Priority { get; } = priority;
	}

	private readonly Dictionary<Type, List<IRequestHandler>> _requestHandlers = [];

	public void MakeRequest<TRequest>(TRequest request) where TRequest : IRequest
	{
		if (!_requestHandlers.TryGetValue(typeof(TRequest), out var handlers))
		{
			return;
		}

		foreach (var handler in handlers)
		{
			if (request.IsConsumed) break;
			if (handler is not RequestHandler<TRequest> typedHandler) continue;
			typedHandler.Callback.Invoke(request);
		}
	}

	public void RegisterHandler<TRequest>(
		Action<TRequest> handler,
		int priority = 0
	) where TRequest : IRequest
	{
		if (!_requestHandlers.TryGetValue(typeof(TRequest), out var handlers))
		{
			handlers = [];
			_requestHandlers.Add(typeof(TRequest), handlers);
		}

		// TODO: Insert in priority order
		handlers.Add(new RequestHandler<TRequest>(handler, priority));
	}
}