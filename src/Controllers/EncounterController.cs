using Data;
using Data.Definitions.Entities;
using Debug;
using Microsoft.EntityFrameworkCore;
using Requests;
using Requests.Definitions;
using View.Definitions;
using View.Views;

namespace Controllers;

public class EncounterController
{
	private readonly SessionFactory _sessionFactory;
	private readonly IViewManager _viewManager;

	public EncounterController(
		IRequestListener listener,
		IViewManager viewManager,
		SessionFactory sessionFactory
	)
	{
		_sessionFactory = sessionFactory;
		_viewManager = viewManager;
		listener.RegisterHandler<CharacterInteractionRequest>(HandleCharacterInteractionRequest);
	}

	private void HandleCharacterInteractionRequest(CharacterInteractionRequest request)
	{
		_viewManager.ShowView(new EncounterView(1));
	}
}