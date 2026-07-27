using Newtonsoft.Json;

namespace Game.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Door : Entity
{
	[JsonProperty("destination_id")]
	public string DestinationId = "";
	[JsonProperty("call_to_action")]
	public string CallToAction = "";
}