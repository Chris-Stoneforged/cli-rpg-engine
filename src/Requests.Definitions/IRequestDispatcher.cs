namespace Requests.Definitions;

public interface IRequestDispatcher
{
	void MakeRequest<TRequest>(TRequest request) where TRequest : IRequest;
}