using Models.Definitions;
using View.Definitions;

namespace View.Menu.Displays;

public class InventoryItemDisplay(IInventoryItem inventoryItem) : IMenuOptionDisplay
{
	public string DisplayMarkup => $"{inventoryItem.Item.Name} - {inventoryItem.Quantity}";
}