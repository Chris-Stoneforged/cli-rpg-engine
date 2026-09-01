using Data;
using Data.Definitions.Entities;
using Debug;
using Microsoft.EntityFrameworkCore;
using Requests;
using Requests.Definitions;

namespace Controllers;

public class InventoryController
{
	private readonly SessionFactory _sessionFactory;

	public InventoryController(IRequestListener listener, SessionFactory sessionFactory)
	{
		_sessionFactory = sessionFactory;
		listener.RegisterHandler<PickUpItemRequest>(HandlePickUpItemRequest);
	}

	private void HandlePickUpItemRequest(PickUpItemRequest request)
	{
		using var db = _sessionFactory.GetSession();

		var pickup = db.ItemPickups
			.Include(p => p.Item)
			.Include(p => p.Location)
			.ThenInclude(l => l.ItemPickups)
			.FirstOrDefault(p => p.Id == request.PickupId);

		if (pickup == null)
		{
			DebugLog.Error($"ItemPickup with Id {request.PickupId} does not exist");
			return;
		}

		if (request.Amount > pickup.Quantity)
		{
			DebugLog.Error("Trying to pick up more of an item than is available");
			return;
		}

		var currentLocation = db.Core.FirstOrDefault()?.CurrentLocation;
		if (currentLocation == null)
		{
			DebugLog.Error("Could not get current location");
			return;
		}

		if (pickup.Location != currentLocation)
		{
			DebugLog.Error("Trying to pick up item that is not in the current location");
			return;
		}

		var existingEntry = db.InventoryEntries
			.FirstOrDefault(e => e.ItemId == pickup.ItemId);
		if (existingEntry == null)
		{
			DebugLog.Info("Item does not exist in inventory. Adding new entry");
			db.InventoryEntries.Add(
				new InventoryEntry()
				{
					Item = pickup.Item,
					Quantity = request.Amount
				});
		}
		else
		{
			DebugLog.Info("Item exists in inventory. Adding quantity");
			existingEntry.Quantity += pickup.Quantity;
		}

		if (request.Amount == pickup.Quantity)
		{
			currentLocation.ItemPickups.Remove(pickup);
		}
		else
		{
			pickup.Quantity -= request.Amount;
		}

		db.SaveChanges();
	}
}