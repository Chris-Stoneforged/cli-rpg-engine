using Requests.Definitions;

namespace Requests;

public abstract class AGameRequest : IRequest
{
	public bool IsConsumed { get; private set; }

	public void ConsumeRequest()
	{
		IsConsumed = true;
	}
}