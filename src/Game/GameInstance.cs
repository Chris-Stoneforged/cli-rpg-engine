using Controllers;
using Events;
using Requests;
using View;
using Data;
using View.Views;
using Debug;
using View.Definitions;

namespace Game;

public class GameInstance
{
	private readonly ViewManager _viewManager;
	private readonly RequestManager _requestManager;
	private readonly EventManager _eventManager;
	private readonly SessionFactory _sessionFactory;
	private readonly SaveManager _saveManager;

	private readonly LocationController _locationController;
	private readonly InventoryController _inventoryController;
	private readonly EncounterController _encounterController;

	private readonly string _campaignPath;

	public GameInstance(string campaignPath)
	{
		_campaignPath = campaignPath;

		_eventManager = new EventManager();
		_requestManager = new RequestManager();
		_sessionFactory = new SessionFactory(campaignPath);
		_saveManager = new SaveManager(campaignPath);
		_viewManager = new ViewManager(_requestManager, _sessionFactory);

		var cachedLoadGameView = new LoadGameView(_saveManager.SavePaths);
		var cachedMainMenuView = new MainMenuView(_saveManager.SavePaths);
		_viewManager.CacheView(ViewKey.LOAD_GAME, cachedLoadGameView);
		_viewManager.CacheView(ViewKey.MAIN_MENU, cachedMainMenuView);

		_locationController = new LocationController(_requestManager, _sessionFactory);
		_inventoryController = new InventoryController(_requestManager, _sessionFactory);
		_encounterController = new EncounterController(_requestManager, _viewManager, _sessionFactory);

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
		_viewManager.ShowCachedView(ViewKey.MAIN_MENU);
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
		var path = _saveManager.CreateSaveProfile();
		if (path == null)
		{
			return;
		}

		_sessionFactory.SetDatabasePath(path);
		EnterGame();
	}

	void HandleSaveGameRequest(SaveGameRequest request)
	{
		// TODO: Something?
	}

	void HandleLoadGameRequest(LoadGameRequest request)
	{
		_sessionFactory.SetDatabasePath(request.SavePath);
		EnterGame();
	}

	void HandleReturnToMainMenuRequest(ReturnToMainMenuRequest request)
	{
		EnterMainMenu();
	}
}