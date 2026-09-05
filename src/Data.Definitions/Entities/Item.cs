using Core.Definitions.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Item : DbEntity
{
	[JsonProperty("name")]
	public string Name { get; set; } = "";
	[JsonProperty("description")]
	public string Description { get; set; } = "";
	[JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))]
	public ItemType Type { get; set; } = ItemType.REGULAR;

	public virtual ICollection<ItemPickup> ItemPickups { get; set; } = [];
	public virtual ICollection<InventoryEntry> InventoryEntries { get; set; } = [];

	public override string Repr()
	{
		return Name;
	}
}