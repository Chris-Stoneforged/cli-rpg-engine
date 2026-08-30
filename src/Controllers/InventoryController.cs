using Data.Definitions.Entities;
using Debug;
using Models;
using Models.Definitions;
using Requests;

namespace Controllers;

public class InventoryController : AController
{
	public InventoryController(ControllerContext ctx) : base(ctx)
	{
		_ctx.RequestListener.RegisterHandler<PickUpItemRequest>(HandlePickUpItemRequest);
	}

	private void HandlePickUpItemRequest(PickUpItemRequest request)
	{
		var locationModel = _ctx.ModelGetter.GetModel<ILocationModel>();
		if (locationModel == null)
		{
			DebugLog.Error("HandlePickUpItemRequest - could not get Location model");
			return;
		}

		if (request.Pickup.Location != locationModel.CurrentLocation)
		{
			DebugLog.Error("Trying to pick up item that is not in the current location");
			return;
		}

		_ctx.ModelUpdater.UpdateModel<InventoryModel>(
			inventory => AddItemToInventory(inventory, request.Pickup.Item, request.Pickup.Quantity)
		);
	}

	public void AddItemToInventory(InventoryModel inventory, Item item, int quantity = 1)
	{
		var existing = inventory.Items.FirstOrDefault(i => i.Item.Id == item.Id);
		if (existing != null)
		{
			existing.Quantity += quantity;
			return;
		}

		inventory.Items.Add(new InventoryItem()
		{
			Item = item,
			Quantity = quantity
		});
	}

	public void RemoveItemFromInventory(InventoryModel inventory, Item item, int quantity = 1)
	{
		var existing = inventory.Items.FirstOrDefault(i => i.Item.Id == item.Id);
		if (existing == null)
		{
			DebugLog.Warn($"Trying to remove item {item.Id} that is not in inventory");
			return;
		}

		if (existing.Quantity < quantity)
		{
			DebugLog.Warn($"Trying to remove {quantity} of {item.Id}, but there are only {existing.Quantity} in inventory");
			return;
		}

		if (existing.Quantity == quantity)
		{
			inventory.Items.Remove(existing);
			return;
		}

		existing.Quantity -= quantity;
	}
}