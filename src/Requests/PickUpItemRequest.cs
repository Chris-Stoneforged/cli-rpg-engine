using Data.Definitions.Entities;

namespace Requests;

public class PickUpItemRequest(ItemPickup itemPickup) : AGameRequest
{
	public ItemPickup Pickup { get; } = itemPickup;
}