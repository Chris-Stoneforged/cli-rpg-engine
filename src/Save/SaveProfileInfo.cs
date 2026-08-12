using Newtonsoft.Json;
using Save.Definitions;

namespace Save;

[JsonObject(MemberSerialization.OptIn)]
public class SaveProfile : ISaveProfile
{
	public string Name { get => _name; set { _name = value; } }
	public string Path { get; set; } = "";
	public DateTime LastSavedTime
	{
		get => DateTime.TryParse(_lastSavedTime, out var dateTime) ? dateTime : DateTime.MinValue;
		set
		{
			_lastSavedTime = value.ToString();
		}
	}

	[JsonProperty("name")]
	private string _name = "";

	[JsonProperty("last_saved_time")]
	private string _lastSavedTime = "";
}