using Newtonsoft.Json;

namespace Data.Definitions.Entities;

[JsonObject(MemberSerialization.OptIn)]
public class Entity : IEquatable<Entity>
{
	[JsonProperty("id")]
	public int Id { get; set; }

	public bool Equals(Entity? other)
	{
		return other != null && other.GetType() == GetType() && other.Id == Id;
	}
}