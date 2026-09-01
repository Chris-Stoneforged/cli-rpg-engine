using Data.Definitions.Entities;
using View.Definitions;

namespace View.Menu.Displays;

public class InventoryEntryDisplay(InventoryEntry inventoryEntry) : IMenuOptionDisplay
{
	public string DisplayMarkup => $"{inventoryEntry.Item?.Name} - {inventoryEntry.Quantity}";
}