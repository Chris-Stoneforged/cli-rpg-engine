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

		if (pickup.Location == null)
		{
			DebugLog.Error($"ItemPickup with Id {pickup.Id} has no location");
			return;
		}

		if (request.Amount > pickup.Quantity)
		{
			DebugLog.Error("Trying to pick up more of an item than is available");
			return;
		}

		var core = db.Core
			.Include(c => c.PlayerCharacter)
			.ThenInclude(c => c.Location)
			.FirstOrDefault();
		if (core == null)
		{
			DebugLog.Error("Could not get core");
			return;
		}

		if (pickup.Location != core.PlayerCharacter?.Location)
		{
			DebugLog.Error("Trying to pick up item that is not in the current location");
			return;
		}

		var existingEntry = core.PlayerCharacter.InventoryEntries
			.FirstOrDefault(e => e.ItemId == pickup.ItemId);
		if (existingEntry == null)
		{
			DebugLog.Info("Item does not exist in inventory. Adding new entry");
			core.PlayerCharacter.InventoryEntries.Add(
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
			pickup.Location.ItemPickups.Remove(pickup);
		}
		else
		{
			pickup.Quantity -= request.Amount;
		}

		db.SaveChanges();
	}
}