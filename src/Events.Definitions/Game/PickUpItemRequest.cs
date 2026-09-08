namespace Events.Definitions.Game;

public class PickUpItemEvent(int pickupId, string itemName, int amount) : AEvent
{
	public int PickupId { get; } = pickupId;
	public string ItemName { get; } = itemName;
	public int Amount { get; } = amount;
}