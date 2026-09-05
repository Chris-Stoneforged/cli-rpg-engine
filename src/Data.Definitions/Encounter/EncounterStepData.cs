using Core.Definitions.Enums;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Definitions.Encounter;

[JsonObject(MemberSerialization.OptIn)]
[JsonConverter(typeof(JsonSubtypes), nameof(Type))]
[JsonSubtypes.KnownSubType(typeof(DialogueStepData), EncounterStepType.DIALOGUE)]
public class EncounterStepData : DbEntity
{
	[JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))]
	public EncounterStepType Type { get; set; }

	public virtual Encounter? Encounter { get; set; }
	public int EncounterId { get; set; }

	public override string Repr()
	{
		return Type.ToString();
	}
}