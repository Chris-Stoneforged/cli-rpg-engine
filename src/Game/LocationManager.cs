using Game.Requests;

namespace Game;

public class LocationManager
{
	public LocationManager()
	{
		GameContext.RequestProcessor.RegisterHandler<OpenDoorRequest>(HandleOpenDoorRequest);
	}

	private void HandleOpenDoorRequest(OpenDoorRequest request)
	{
		if (request.LocationId != GameContext.WorldState.CurrentLocationId) return;
		GameContext.WorldState.CurrentLocationId = request.DestinationId;
	}
}