using Core.Definitions.Enums;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Definitions.Triggers;

[JsonObject(MemberSerialization.OptIn)]
[JsonConverter(typeof(JsonSubtypes), nameof(Type))]
[JsonSubtypes.KnownSubType(typeof(CharacterInteractionTrigger), TriggerType.CHARACTER_INTERACTION)]
public class Trigger
{
	public int Id { get; set; }

	[JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))]
	public TriggerType Type { get; set; }
}