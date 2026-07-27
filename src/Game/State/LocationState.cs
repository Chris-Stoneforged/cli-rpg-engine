using Game.Requests;

namespace Game.State;

public class LocationState : IGameState
{
	public void Loop()
	{
		var locationId = GameContext.WorldState.CurrentLocationId;
		var location = GameContext.ResourceManager.GetLocation(locationId);
		if (location == null)
		{
			return;
		}

		Console.WriteLine(location.Description);

		var menu = new Menu.Menu();
		foreach (var door in location.Doors)
		{
			var request = new OpenDoorRequest(location.Id, door.DestinationId, door.Id);
			menu.AddOption(door.CallToAction, () => GameContext.RequestProcessor.MakeRequest(request));
		}
		menu.Execute();
	}
}