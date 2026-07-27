using Newtonsoft.Json;

namespace Game.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Entity
{
	[JsonProperty("id")]
	public string Id = "Hello";
	[JsonProperty("tags")]
	public string[] Tags = [];
}