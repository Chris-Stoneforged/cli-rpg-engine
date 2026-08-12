using Events.Definitions;
using Models.Definitions;
using Requests.Definitions;
using Resources.Definitions;
using Save.Definitions;

namespace Controllers;

// Right now, controllers can get and update models,
// send and listen to events, receive commands, load
// entity resources, and save and load models. That's
// a lot of responsibilities, maybe this should change?
public class ControllerContext(
	IModelGetter modelGetter,
	IModelUpdater modelUpdater,
	IRequestListener requestListener,
	IEventDispatcher eventDispatcher,
	IEventListener eventListener,
	IEntityLoader entityLoader,
	ISaveManager saveSystem
)
{
	public IModelGetter ModelGetter { get; } = modelGetter;
	public IModelUpdater ModelUpdater { get; } = modelUpdater;
	public IRequestListener RequestListener { get; } = requestListener;
	public IEventDispatcher EventDispatcher { get; } = eventDispatcher;
	public IEventListener EventListener { get; } = eventListener;
	public IEntityLoader EntityLoader { get; } = entityLoader;
	public ISaveManager SaveSystem { get; } = saveSystem;
}