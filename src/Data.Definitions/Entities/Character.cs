using Newtonsoft.Json;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Character : Entity
{
	[JsonProperty("name")]
	public string Name { get; set; } = "";
	[JsonProperty("location_id")]
	public int LocationId { get; set; }

	public Location? Location { get; set; }
	public ICollection<InventoryEntry> InventoryEntries = [];
}