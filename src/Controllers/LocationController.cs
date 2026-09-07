using Data;
using Debug;
using Events.Definitions;
using Events.Definitions.Game;

namespace Controllers;

public class LocationController
{
	private readonly SessionFactory _sessionFactory;

	public LocationController(IEventHandler handler, SessionFactory sessionFactory)
	{
		_sessionFactory = sessionFactory;
		handler.Register<OpenDoorEvent>(HandleOpenDoorEvent);
	}

	private void HandleOpenDoorEvent(OpenDoorEvent request)
	{
		using var db = _sessionFactory.GetSession();

		var door = db.Doors.FirstOrDefault(d => d.Id == request.DoorId);
		if (door == null)
		{
			DebugLog.Error($"Could not find door with Id {request.DoorId}");
			return;
		}

		if (door.From == null)
		{
			DebugLog.Error($"Door with Id {door.Id} does not have a From location");
			return;
		}

		if (door.To == null)
		{
			DebugLog.Error("Door leads to null");
			return;
		}

		var core = db.Core.FirstOrDefault();
		if (core == null)
		{
			DebugLog.Error("Could not get core");
			return;
		}

		if (door.From != core.PlayerCharacter?.Location)
		{
			DebugLog.Error("Attempting to use door that is not in the current location");
			return;
		}

		core.PlayerCharacter.Location = door.To;
		db.SaveChanges();
	}
}