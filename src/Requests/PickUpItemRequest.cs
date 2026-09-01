namespace Requests;

public class PickUpItemRequest(int pickupId, int amount) : AGameRequest
{
	public int PickupId { get; } = pickupId;
	public int Amount { get; } = amount;
}