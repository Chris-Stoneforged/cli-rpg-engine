using Newtonsoft.Json;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Character : DbEntity
{
	[JsonProperty("name")]
	public string Name { get; set; } = "";
	[JsonProperty("location_id")]
	public int LocationId { get; set; }

	public virtual Location? Location { get; set; }
	public virtual ICollection<InventoryEntry> InventoryEntries { get; set; } = [];

	public override string Repr()
	{
		return Name;
	}
}