namespace Events.Definitions.Game;

public class PickUpItemEvent(int pickupId, int amount) : AEvent
{
	public int PickupId { get; } = pickupId;
	public int Amount { get; } = amount;
}