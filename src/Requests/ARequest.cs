using Requests.Definitions;

namespace Requests;

public abstract class ARequest : IRequest
{
	public bool IsConsumed { get; private set; }

	public void ConsumeRequest()
	{
		IsConsumed = true;
	}
}