using Data.Definitions.Entities;
using View.Definitions;

namespace View.Menu.Displays;

public class ItemPickupDisplay(ItemPickup pickup) : IMenuOptionDisplay
{
	public string DisplayMarkup => $"{pickup.Item?.Name} ({pickup.Quantity})";
}