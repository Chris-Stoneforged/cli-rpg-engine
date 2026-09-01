namespace Data.Definitions.Entities;

public class InventoryEntry : Entity
{
	public Item? Item { get; set; }
	public int ItemId { get; set; }

	public Character? Owner { get; set; }
	public int OwnerId { get; set; }

	public int Quantity { get; set; }
}