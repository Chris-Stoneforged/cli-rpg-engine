using Save.Definitions;
using Newtonsoft.Json;

namespace Save;

public class LocationSaveData : ISaveData
{
	[JsonProperty("current_location_id")]
	public string CurrentLocationId = "0000";
}