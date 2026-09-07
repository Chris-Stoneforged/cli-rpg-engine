using Data;
using Data.Definitions.Entities;
using Debug;
using Events.Definitions;
using Events.Definitions.Game;

namespace Controllers;

public class InventoryController
{
	private readonly SessionFactory _sessionFactory;

	public InventoryController(IEventHandler handler, SessionFactory sessionFactory)
	{
		_sessionFactory = sessionFactory;
		handler.Register<PickUpItemEvent>(HandlePickUpItemEvent);
	}

	private void HandlePickUpItemEvent(PickUpItemEvent @event)
	{
		using var db = _sessionFactory.GetSession();

		var pickup = db.ItemPickups.FirstOrDefault(p => p.Id == @event.PickupId);

		if (pickup == null)
		{
			DebugLog.Error($"ItemPickup with Id {@event.PickupId} does not exist");
			return;
		}

		if (pickup.Location == null)
		{
			DebugLog.Error($"ItemPickup with Id {pickup.Id} has no location");
			return;
		}

		if (@event.Amount > pickup.Quantity)
		{
			DebugLog.Error("Trying to pick up more of an item than is available");
			return;
		}

		var core = db.Core.FirstOrDefault();
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
					Quantity = @event.Amount
				});
		}
		else
		{
			DebugLog.Info("Item exists in inventory. Adding quantity");
			existingEntry.Quantity += pickup.Quantity;
		}

		if (@event.Amount == pickup.Quantity)
		{
			pickup.Location.ItemPickups.Remove(pickup);
		}
		else
		{
			pickup.Quantity -= @event.Amount;
		}

		db.SaveChanges();
	}
}