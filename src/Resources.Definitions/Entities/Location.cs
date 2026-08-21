using Newtonsoft.Json;

namespace Resources.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Location : Entity
{
	[JsonProperty("name")]
	public string Name = "";
	[JsonProperty("doors")]
	public Door[] Doors = [];
}