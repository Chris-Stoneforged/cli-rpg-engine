using Data.Definitions;
using Events.Definitions;
using Requests.Definitions;

namespace Controllers;

// Right now, controllers can get and update models,
// send and listen to events, receive commands, load
// entity resources, and save and load models. That's
// a lot of responsibilities, maybe this should change?
public class ControllerContext(
	IRequestListener requestListener,
	IEventDispatcher eventDispatcher,
	IEventListener eventListener,
	ISessionFactory sessionFactory
)
{
	public IRequestListener RequestListener { get; } = requestListener;
	public IEventDispatcher EventDispatcher { get; } = eventDispatcher;
	public IEventListener EventListener { get; } = eventListener;
	public ISessionFactory DataFactory { get; } = sessionFactory;
}