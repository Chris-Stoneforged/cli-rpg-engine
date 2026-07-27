using Game.Entities;

using Newtonsoft.Json;

namespace Game.Resources;

public class ResourceManager
{
	private bool _campaignLoaded;

	public bool LoadCampaign(string campaignPath)
	{
		if (_campaignLoaded)
		{
			return false;
		}

		if (!Directory.Exists(campaignPath))
		{
			return false;
		}

		var locationsPath = Path.Combine(campaignPath, "locations");
		foreach (var path in Directory.EnumerateFiles(locationsPath))
		{
			var contents = File.ReadAllText(path);
			var location = JsonConvert.DeserializeObject<Location>(contents);
			if (location == null)
			{
				// TODO: Print error
				continue;
			}

			_locations.Add(location.Id, location);
		}

		_campaignLoaded = true;
		return true;
	}

	private readonly Dictionary<string, Location> _locations = [];

	public Location? GetLocation(string locationId)
	{
		return _locations.TryGetValue(locationId, out var location) ? location : null;
	}
}