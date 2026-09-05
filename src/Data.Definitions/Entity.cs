using Newtonsoft.Json;

namespace Data.Definitions;

[JsonObject(MemberSerialization.OptIn)]
public abstract class DbEntity : IEquatable<DbEntity>
{
	[JsonProperty("id")]
	public int Id { get; set; }

	public bool Equals(DbEntity? other)
	{
		return other != null && other.GetType() == GetType() && other.Id == Id;
	}

	public abstract string Repr();
}