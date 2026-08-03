namespace Requests.Definitions;

public interface IRequestMaker
{
	void MakeRequest<TRequest>(TRequest request) where TRequest : IRequest;
}