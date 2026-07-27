using Game.Events;
using Game.Requests;
using Game.Resources;
using Game.UserInterface;

namespace Game;

public class GameContext
{
	public static UIController UIController
	{
		get
		{
			return s_instance == null || s_instance._uiController == null
				? throw new Exception("Trying to access UIController from GameContext before creation")
				: s_instance._uiController;
		}
	}

	public static RequestProcessor RequestProcessor
	{
		get
		{
			return s_instance == null || s_instance._requestProcessor == null
				? throw new Exception("Trying to access RequestProcessor from GameContext before creation")
				: s_instance._requestProcessor;
		}
	}

	public static WorldState WorldState
	{
		get
		{
			return s_instance == null || s_instance._worldState == null
				? throw new Exception("Trying to access WorldState from GameContext before creation")
				: s_instance._worldState;
		}
	}

	public static ResourceManager ResourceManager
	{
		get
		{
			return s_instance == null || s_instance._resourceManager == null
				? throw new Exception("Trying to access ResourceManager from GameContext before creation")
				: s_instance._resourceManager;
		}
	}

	public static EventDispatcher EventDispatcher
	{
		get
		{
			return s_instance == null || s_instance._eventDispatcher == null
				? throw new Exception("Trying to access EventDispatcher from GameContext before creation")
				: s_instance._eventDispatcher;
		}
	}

	private static GameContext? s_instance;

	private UIController? _uiController = null;
	private RequestProcessor? _requestProcessor = null;
	private WorldState? _worldState = null;
	private ResourceManager? _resourceManager = null;
	private EventDispatcher? _eventDispatcher = null;

	public static void Create(
		UIController uiController,
		RequestProcessor requestProcessor,
		WorldState worldState,
		ResourceManager resourceManager,
		EventDispatcher eventDispatcher
	)
	{
		if (s_instance != null)
		{
			return;
		}

		s_instance = new()
		{
			_uiController = uiController,
			_requestProcessor = requestProcessor,
			_worldState = worldState,
			_resourceManager = resourceManager,
			_eventDispatcher = eventDispatcher
		};
	}
}