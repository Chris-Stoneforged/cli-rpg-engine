using Controllers;
using Events;
using Models;
using Requests;
using View;
using Save;
using Data;
using View.Views;

namespace Game;

public class GameInstance
{
	private readonly ViewManager _viewManager;
	private readonly RequestManager _requestManager;
	private readonly EventManager _eventManager;
	private readonly ModelManager _modelManager;
	private readonly SaveManager _saveManager;
	private readonly ContextFactory _contextFactory;

	private readonly LocationController _locationController;
	private readonly InventoryController _inventoryController;

	public GameInstance(string campaignPath)
	{
		_modelManager = ModelManager.Create();
		_eventManager = new EventManager();
		_requestManager = new RequestManager();
		_saveManager = new SaveManager();
		_contextFactory = new ContextFactory(campaignPath);
		_viewManager = new ViewManager(_requestManager, _modelManager, _saveManager);

		var ctx = new ControllerContext(
			_modelManager,
			_modelManager,
			_requestManager,
			_eventManager,
			_eventManager,
			_contextFactory,
			_saveManager
		);

		_locationController = new LocationController(ctx);
		_inventoryController = new InventoryController(ctx);

		_requestManager.RegisterHandler<QuitGameRequest>(HandleQuitGameRequest);
		_requestManager.RegisterHandler<NewGameRequest>(HandleCreateSaveRequest);
		_requestManager.RegisterHandler<SaveGameRequest>(HandleSaveGameRequest);
		_requestManager.RegisterHandler<LoadGameRequest>(HandleLoadGameRequest);
		_requestManager.RegisterHandler<ReturnToMainMenuRequest>(HandleReturnToMainMenuRequest);

		EnterMainMenu();
	}

	private bool _gameRunning = true;

	public async Task Run()
	{
		while (_gameRunning)
		{
			await _viewManager.Show();
		}
	}

	void EnterMainMenu()
	{
		_viewManager.ResetStack();
		_viewManager.ShowView(new MainMenuView());
	}

	void EnterGame()
	{
		_viewManager.ResetStack();
		_viewManager.ShowView(new WorldView());
	}

	void HandleQuitGameRequest(QuitGameRequest request)
	{
		_gameRunning = false;
	}

	void HandleCreateSaveRequest(NewGameRequest request)
	{
		if (_saveManager.CreateNewSaveFile())
		{
			EnterGame();
		}
	}

	void HandleSaveGameRequest(SaveGameRequest request)
	{
		_saveManager.SaveGameData();
	}

	void HandleLoadGameRequest(LoadGameRequest request)
	{
		if (_saveManager.LoadSaveProfile(request.ProfileInfo))
		{
			EnterGame();
		}
	}

	void HandleReturnToMainMenuRequest(ReturnToMainMenuRequest request)
	{
		EnterMainMenu();
	}
}