using Newtonsoft.Json;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Location : Entity
{
	[JsonProperty("name")]
	public string Name { get; set; } = "";

	public List<Door> DoorsOut { get; set; } = [];
	public List<Door> DoorsIn { get; set; } = [];
	public List<ItemPickup> ItemPickups { get; set; } = [];
}