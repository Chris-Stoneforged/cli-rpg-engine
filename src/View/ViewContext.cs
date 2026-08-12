using Models.Definitions;
using Requests.Definitions;
using View.Definitions;

namespace View;

public class ViewContext(
	IViewManager viewManager,
	IRequestDispatcher requestDispatcher,
	IModelGetter modelGetter
)
{
	public readonly IModelGetter ModelGetter = modelGetter;
	public readonly IViewManager ViewManager = viewManager;
	public readonly IRequestDispatcher RequestDispatcher = requestDispatcher;
}