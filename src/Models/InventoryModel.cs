using Models.Definitions;

namespace Models;

public class InventoryModel : IInventoryModel
{
	public IReadOnlyList<IInventoryItem> InventoryItems => Items;
	public readonly List<InventoryItem> Items = [];
}