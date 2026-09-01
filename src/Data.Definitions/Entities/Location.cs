using Newtonsoft.Json;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Location : Entity
{
	[JsonProperty("name")]
	public string Name { get; set; } = "";

	public ICollection<Door> DoorsOut { get; set; } = [];
	public ICollection<Door> DoorsIn { get; set; } = [];
	public ICollection<ItemPickup> ItemPickups { get; set; } = [];
	public ICollection<Character> Characters { get; set; } = [];
}