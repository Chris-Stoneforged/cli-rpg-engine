using Newtonsoft.Json;

namespace Data.Definitions.Triggers;

[JsonObject(MemberSerialization.OptIn)]
public class CharacterInteractionTrigger : Trigger
{
	[JsonProperty("character_id")]
	public int CharacterId { get; set; }
}