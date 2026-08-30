using Newtonsoft.Json;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Door : Entity
{
	public Location? From { get; set; }
	public Location? To { get; set; }

	[JsonProperty("from_id")]
	public int FromId { get; set; }
	[JsonProperty("to_id")]
	public int ToId { get; set; }
	[JsonProperty("call_to_action")]
	public string CallToAction { get; set; } = "";
}