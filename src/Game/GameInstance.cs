using Controllers;
using Events;
using Models;
using Requests;
using UserInterface;
using View;
using Resources;
using View.Definitions;

namespace Game;

public class GameInstance(
	ResourceManager resourceManager,
	UserInterface.UserInterface ui,
	RequestRegister requestRegister,
	EventDispatcher eventDispatcher,
	ModelRegister modelRegister
)
{
	private readonly Stack<IView> _viewStack = new();
	private readonly ResourceManager _resourceManager = resourceManager;
	private readonly UserInterface.UserInterface _ui = ui;
	private readonly RequestRegister _requestRegister = requestRegister;
	private readonly EventDispatcher _eventDispatcher = eventDispatcher;
	private readonly ModelRegister _modelRegister = modelRegister;

	private bool _gameRunning = true;

	public static bool TryCreate(string campaignPath, out GameInstance? instance)
	{
		instance = null;

		var ui = new UserInterface.UserInterface(Console.Out, Console.In);
		if (!Path.Exists(campaignPath))
		{
			campaignPath = ui.PushUserInput("Enter path to campagin", [new ValidPathRestriction()]);
		}

		var resourceManager = new ResourceManager();
		if (!resourceManager.LoadCampaign(campaignPath))
		{
			return false;
		}

		var models = new ModelRegister()
			.RegisterModel<LocationModel>();
		var eventDispatcher = new EventDispatcher();
		var requestRegister = new RequestRegister();

		var locationController = new LocationController(requestRegister, models, resourceManager);

		instance = new GameInstance(
			resourceManager,
			ui,
			requestRegister,
			eventDispatcher,
			models
		);

		requestRegister.RegisterHandler<PushViewRequest>(instance.HandlePushViewRequest);
		requestRegister.RegisterHandler<PopViewRequest>(instance.HandlePopViewRequest);
		requestRegister.RegisterHandler<QuitGameRequest>(instance.HandleQuitGameRequest);
		instance.PushView(new MainMenuView());

		return true;
	}

	public void Run()
	{
		while (_gameRunning)
		{
			if (!_viewStack.TryPeek(out var currentState)) break;
			currentState.Loop();
		}
	}

	public void PushView(IView view)
	{
		view.Initialize(_ui, _modelRegister, _requestRegister);
		_viewStack.Push(view);
	}

	public void PopView()
	{
		var view = _viewStack.Pop();
		view.CleanUp();
	}

	void HandlePushViewRequest(PushViewRequest request)
	{
		PushView(request.View);
	}

	void HandlePopViewRequest(PopViewRequest request)
	{
		if (!_viewStack.TryPeek(out var currentState)) return;
		if (currentState == request.View)
		{
			PopView();
		}
	}

	void HandleQuitGameRequest(QuitGameRequest _)
	{
		_gameRunning = false;
	}
}