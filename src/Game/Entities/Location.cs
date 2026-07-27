using Newtonsoft.Json;

namespace Game.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Location : Entity
{
	[JsonProperty("description")]
	public string Description = "";
	[JsonProperty("doors")]
	public Door[] Doors = [];
}