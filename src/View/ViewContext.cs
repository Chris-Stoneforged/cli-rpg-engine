using Data.Definitions;
using Requests.Definitions;
using View.Definitions;

namespace View;

public class ViewContext(
	IViewManager viewManager,
	IRequestDispatcher requestDispatcher,
	ISessionFactory sessionFactory
)
{
	public readonly ISessionFactory SessionFactory = sessionFactory;
	public readonly IViewManager ViewManager = viewManager;
	public readonly IRequestDispatcher RequestDispatcher = requestDispatcher;
}