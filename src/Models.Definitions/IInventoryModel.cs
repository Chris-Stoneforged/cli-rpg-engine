namespace Models.Definitions;

public interface IInventoryModel : IModel
{
	IReadOnlyList<IInventoryItem> InventoryItems { get; }
}