using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Item : Entity
{
	[JsonProperty("name")]
	public string Name { get; set; } = "";
	[JsonProperty("description")]
	public string Description { get; set; } = "";
	[JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))]
	public ItemType Type { get; set; } = ItemType.REGULAR;

	public List<ItemPickup> ItemPickups { get; set; } = [];
	public List<InventoryEntry> InventoryEntries { get; set; } = [];
}