using Newtonsoft.Json;

namespace Data.Definitions;

[JsonObject(MemberSerialization.OptIn)]
public class Door : Entity
{
	[JsonProperty("destination_id")]
	public string DestinationId { get; set; } = "";
	[JsonProperty("call_to_action")]
	public string CallToAction { get; set; } = "";
}