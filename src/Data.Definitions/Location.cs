using Newtonsoft.Json;

namespace Data.Definitions;

[JsonObject(MemberSerialization.OptIn)]
public class Location : Entity
{
	[JsonProperty("name")]
	public string Name { get; set; } = "";
	[JsonProperty("doors")]
	public List<Door> Doors { get; set; } = [];
}