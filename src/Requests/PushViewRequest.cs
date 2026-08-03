namespace Requests;

using View.Definitions;

public class PushViewRequest(IView view) : ARequest
{
	public IView View { get; } = view;
}