using Data;
using Debug;
using Microsoft.EntityFrameworkCore;
using Requests;
using Requests.Definitions;

namespace Controllers;

public class LocationController
{
	private readonly SessionFactory _sessionFactory;

	public LocationController(IRequestListener listener, SessionFactory sessionFactory)
	{
		_sessionFactory = sessionFactory;
		listener.RegisterHandler<OpenDoorRequest>(HandleOpenDoorRequest);
	}

	private void HandleOpenDoorRequest(OpenDoorRequest request)
	{
		using var db = _sessionFactory.GetSession();

		var door = db.Doors
			.Include(d => d.To)
			.Include(d => d.From)
			.FirstOrDefault(d => d.Id == request.DoorId);

		if (door == null)
		{
			DebugLog.Error($"Could not find door with Id {request.DoorId}");
			return;
		}

		var core = db.Core.Include(c => c.CurrentLocation).FirstOrDefault();
		if (core == null)
		{
			DebugLog.Error("Could not get core");
			return;
		}

		if (core.CurrentLocation == null)
		{
			DebugLog.Error("Could not get current location");
			return;
		}

		if (door.From != core.CurrentLocation)
		{
			DebugLog.Error("Attempting to use door that is not in the current location");
			return;
		}

		if (door.To == null)
		{
			DebugLog.Error("Door leads to null");
			return;
		}

		core.CurrentLocation = door.To;
		db.SaveChanges();
	}
}