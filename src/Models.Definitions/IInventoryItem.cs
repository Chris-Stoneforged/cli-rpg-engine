using Data.Definitions.Entities;

namespace Models.Definitions;

public interface IInventoryItem
{
	Item Item { get; }
	int Quantity { get; }
}