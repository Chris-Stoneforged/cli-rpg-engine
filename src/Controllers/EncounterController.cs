using Data;
using Events.Definitions;
using Events.Definitions.Game;
using View.Definitions;
using View.Views;

namespace Controllers;

public class EncounterController
{
	private readonly SessionFactory _sessionFactory;
	private readonly IViewManager _viewManager;

	public EncounterController(
		IEventHandler handler,
		IViewManager viewManager,
		SessionFactory sessionFactory
	)
	{
		_sessionFactory = sessionFactory;
		_viewManager = viewManager;
		handler.Register<CharacterInteractionEvent>(HandleCharacterInteractionEvent);
	}

	private void HandleCharacterInteractionEvent(CharacterInteractionEvent request)
	{
		_viewManager.ShowView(new EncounterView(1));
	}
}