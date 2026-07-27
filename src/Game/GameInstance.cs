using Game.Events;
using Game.Requests;
using Game.Resources;
using Game.State;
using Game.UserInterface;

namespace Game;

public class GameInstance
{
	private readonly Stack<IGameState> _stateStack = new();

	public static bool TryLoad(string campaignPath, out GameInstance instance)
	{
		instance = new GameInstance();

		var ui = new UIController(Console.Out, Console.In);
		if (!Path.Exists(campaignPath))
		{
			campaignPath = ui.PushUserInput("Enter path to campagin", [new ValidPathRestriction()]);
		}

		var resourceManager = new ResourceManager();
		if (!resourceManager.LoadCampaign(campaignPath))
		{
			return false;
		}

		var worldState = new WorldState();
		var locationId = worldState.CurrentLocationId;
		var location = resourceManager.GetLocation(locationId);
		if (location == null)
		{
			return false;
		}

		instance.PushState(new LocationState());

		var eventDispatcher = new EventDispatcher();
		var requestProcessor = new RequestProcessor();
		requestProcessor.RegisterHandler<PushGameStateRequest>(instance.HandlePushStateRequest);

		GameContext.Create(ui, requestProcessor, worldState, resourceManager, eventDispatcher);

		// Initialize game systems
		var locationManager = new LocationManager();

		return true;

	}

	public void Run()
	{
		while (true)
		{
			if (!_stateStack.TryPeek(out var currentState)) break;
			currentState.Loop();
		}
	}

	public void PushState(IGameState state)
	{
		_stateStack.Push(state);
	}

	public void PopState()
	{
		_stateStack.Pop();
	}

	void HandlePushStateRequest(PushGameStateRequest request)
	{
		PushState(request.State);
	}
}