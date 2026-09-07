using Data.Definitions;
using Events.Definitions;
using View.Definitions;

namespace View;

public class ViewContext(
	IViewManager viewManager,
	IEventTrigger eventEmitter,
	ISessionFactory sessionFactory
)
{
	public readonly ISessionFactory SessionFactory = sessionFactory;
	public readonly IViewManager ViewManager = viewManager;
	public readonly IEventTrigger EventEmitter = eventEmitter;
}