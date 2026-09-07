using Controllers;
using Events;
using View;
using Data;
using View.Views;
using View.Definitions;
using Events.Definitions.System;

namespace Game;

public class GameInstance
{
	private readonly ViewManager _viewManager;
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
		_sessionFactory = new SessionFactory(campaignPath);
		_saveManager = new SaveManager(campaignPath);
		_viewManager = new ViewManager(_eventManager, _sessionFactory);

		var cachedLoadGameView = new LoadGameView(_saveManager.SavePaths);
		var cachedMainMenuView = new MainMenuView(_saveManager.SavePaths);
		_viewManager.CacheView(ViewKey.LOAD_GAME, cachedLoadGameView);
		_viewManager.CacheView(ViewKey.MAIN_MENU, cachedMainMenuView);

		_locationController = new LocationController(_eventManager, _sessionFactory);
		_inventoryController = new InventoryController(_eventManager, _sessionFactory);
		_encounterController = new EncounterController(_eventManager, _viewManager, _sessionFactory);

		_eventManager.Register<QuitGameEvent>(HandleQuitGameEvent);
		_eventManager.Register<CreateSaveProfileEvent>(HandleCreateSaveEvent);
		_eventManager.Register<SaveGameEvent>(HandleSaveGameEvent);
		_eventManager.Register<LoadSaveProfileEvent>(HandleLoadGameEvent);
		_eventManager.Register<ReturnToMainMenuEvent>(HandleReturnToMainMenuEvent);

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

	void HandleQuitGameEvent(QuitGameEvent @event)
	{
		_gameRunning = false;
	}

	void HandleCreateSaveEvent(CreateSaveProfileEvent @event)
	{
		var path = _saveManager.CreateSaveProfile();
		if (path == null)
		{
			return;
		}

		_sessionFactory.SetDatabasePath(path);
		EnterGame();
	}

	void HandleSaveGameEvent(SaveGameEvent @event)
	{
		// TODO: Something?
	}

	void HandleLoadGameEvent(LoadSaveProfileEvent @event)
	{
		_sessionFactory.SetDatabasePath(@event.SavePath);
		EnterGame();
	}

	void HandleReturnToMainMenuEvent(ReturnToMainMenuEvent @event)
	{
		EnterMainMenu();
	}
}