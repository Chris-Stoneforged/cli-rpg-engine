using Newtonsoft.Json;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Door : DbEntity
{
	public virtual Location? From { get; set; }
	public virtual Location? To { get; set; }

	[JsonProperty("from_id")]
	public int FromId { get; set; }
	[JsonProperty("to_id")]
	public int ToId { get; set; }
	[JsonProperty("call_to_action")]
	public string CallToAction { get; set; } = "";

	public override string Repr()
	{
		return $"Door from {From?.Repr()} to {To?.Repr()}";
	}
}