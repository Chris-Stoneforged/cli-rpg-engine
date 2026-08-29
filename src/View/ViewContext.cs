using Models.Definitions;
using Requests.Definitions;
using Save.Definitions;
using View.Definitions;

namespace View;

public class ViewContext(
	IViewManager viewManager,
	IRequestDispatcher requestDispatcher,
	IModelGetter modelGetter,
	ISaveManager saveManager
)
{
	public readonly IModelGetter ModelGetter = modelGetter;
	public readonly IViewManager ViewManager = viewManager;
	public readonly IRequestDispatcher RequestDispatcher = requestDispatcher;
	public readonly ISaveManager SaveManager = saveManager; // TODO: Find a way to remove this
}