using Game.Requests;

using Moq;

namespace Game.Test;

public class TestRequests
{
	class TestRequest : ARequest
	{

	}

	class TestRequestHandler(int priority = 1) : IRequestHandler<TestRequest>
	{
		public int Priority { get; } = priority;

		public void HandleRequest(TestRequest request)
		{

		}
	}

	[Fact]
	public void TestRegisterHandler()
	{
		var rp = new RequestRegister();
		var handler = new Mock<TestRequestHandler>();
		rp.RegisterRequestHandler(handler);

		var request = new TestRequest();
		rp.MakeRequest(request);
	}
}