using Newtonsoft.Json;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Location : DbEntity
{
	[JsonProperty("name")]
	public string Name { get; set; } = "";

	public virtual ICollection<Door> DoorsOut { get; set; } = [];
	public virtual ICollection<Door> DoorsIn { get; set; } = [];
	public virtual ICollection<ItemPickup> ItemPickups { get; set; } = [];
	public virtual ICollection<Character> Characters { get; set; } = [];

	public override string Repr()
	{
		return Name;
	}
}