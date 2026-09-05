using Data.Definitions.Entities;
using Newtonsoft.Json;

namespace Data.Definitions.Encounter;

[JsonObject(MemberSerialization.OptIn)]
public class DialogueStepData : EncounterStepData
{
	public virtual Character? Speaker { get; set; }

	[JsonProperty("dialogue")]
	public string Dialogue { get; set; } = "";
	[JsonProperty("speaker_id")]
	public int SpeakerId { get; set; }
	[JsonProperty("next_step_id")]
	public int NextStepId { get; set; } = 0;
}