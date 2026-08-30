using Newtonsoft.Json;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class ItemPickup : Entity
{
	public Location? Location { get; set; }
	public Item? Item { get; set; }

	[JsonProperty("location_id")]
	public int LocationId { get; set; }
	[JsonProperty("item_id")]
	public int ItemId { get; set; }
	[JsonProperty("quantity")]
	public int Quantity { get; set; }
}