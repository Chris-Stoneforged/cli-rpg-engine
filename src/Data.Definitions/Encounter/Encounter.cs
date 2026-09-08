using Data.Definitions.Triggers;
using Newtonsoft.Json;

namespace Data.Definitions.Encounter;

[JsonObject(MemberSerialization.OptIn)]
public class Encounter : DbEntity
{
	[JsonProperty("repeatable")]
	public bool IsRepeatable { get; set; }
	[JsonProperty("repeat_step")]
	public int RepeatStep { get; set; } = 1;
	[JsonProperty("steps")]
	public virtual ICollection<EncounterStepData> Steps { get; set; } = [];
	[JsonProperty("trigger")]
	public virtual Trigger? Trigger { get; set; }

	public bool IsComplete { get; set; }

	public override string Repr()
	{
		return $"Encounter {Id}";
	}
}