using Newtonsoft.Json;

namespace Resources.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Location : Entity
{
	[JsonProperty("description")]
	public string Description = "";
	[JsonProperty("doors")]
	public Door[] Doors = [];
}