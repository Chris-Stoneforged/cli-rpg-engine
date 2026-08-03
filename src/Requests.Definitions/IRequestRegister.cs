namespace Requests.Definitions;

public interface IRequestRegister
{
	public void RegisterHandler<TRequest>(
		Action<TRequest> handler,
		int priority = 0
	) where TRequest : IRequest;
}