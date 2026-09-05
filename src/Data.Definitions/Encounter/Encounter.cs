using Newtonsoft.Json;

namespace Data.Definitions.Encounter;

[JsonObject(MemberSerialization.OptIn)]
public class Encounter : DbEntity
{
	[JsonProperty("repeatable")]
	public bool Repeatable { get; set; }
	[JsonProperty("steps")]
	public virtual ICollection<EncounterStepData> Steps { get; set; } = [];

	public bool Completed { get; set; }

	public override string Repr()
	{
		return $"Encounter {Id}";
	}
}