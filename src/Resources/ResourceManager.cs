using Newtonsoft.Json;
using Resources.Definitions;
using Resources.Definitions.Entities;

namespace Resources;

public class ResourceManager : IEntityLoader
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

			_entities.Add(location.Id, location);
		}

		_campaignLoaded = true;
		return true;
	}

	private readonly Dictionary<string, Entity> _entities = [];

	public TEntity? LoadEntity<TEntity>(string id) where TEntity : Entity
	{
		return _entities.TryGetValue(id, out var entity) && entity is TEntity typedEntity ? typedEntity : null;
	}

}