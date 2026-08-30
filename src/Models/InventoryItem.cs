using Data.Definitions.Entities;
using Models.Definitions;

namespace Models;

public class InventoryItem : IInventoryItem
{
	public required Item Item { get; set; }
	public required int Quantity { get; set; }
}