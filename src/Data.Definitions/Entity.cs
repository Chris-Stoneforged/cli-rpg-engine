using Newtonsoft.Json;

namespace Data.Definitions;

[JsonObject(MemberSerialization.OptIn)]
public class Entity
{
	[JsonProperty("id")]
	public string Id { get; set; } = "";
}