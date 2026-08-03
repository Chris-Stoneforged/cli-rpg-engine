using View.Definitions;

namespace Requests;

public class PopViewRequest(IView view) : ARequest
{
	public IView View { get; } = view;
}