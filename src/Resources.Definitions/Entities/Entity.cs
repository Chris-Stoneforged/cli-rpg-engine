using Newtonsoft.Json;

namespace Resources.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Entity
{
	[JsonProperty("id")]
	public string Id = "";
	[JsonProperty("tags")]
	public string[] Tags = [];
}