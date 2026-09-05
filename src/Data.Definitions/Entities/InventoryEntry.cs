namespace Data.Definitions.Entities;

public class InventoryEntry : DbEntity
{
	public virtual Item? Item { get; set; }
	public int ItemId { get; set; }

	public virtual Character? Owner { get; set; }
	public int OwnerId { get; set; }

	public int Quantity { get; set; }

	public override string Repr()
	{
		return $"{Item?.Name} ({Quantity})";
	}
}