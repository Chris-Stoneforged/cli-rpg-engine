using Debug;
using Newtonsoft.Json;
using Resources.Definitions;
using Resources.Definitions.Entities;
using View.Definitions;

namespace Resources;

public class ResourceManager : IEntityLoader
{
	private bool _campaignLoaded;

	public Loader<bool> LoadCampaign(string campaignPath)
	{
		async Task<bool> DoLoad(ILoadContext ctx)
		{
			if (_campaignLoaded)
			{
				return false;
			}

			if (!Directory.Exists(campaignPath))
			{
				return false;
			}

			ctx.SetLoadText("Loading resources...");

			var locationsPath = Path.Combine(campaignPath, "locations");
			foreach (var path in Directory.EnumerateFiles(locationsPath))
			{
				var contents = await File.ReadAllTextAsync(path);
				var location = JsonConvert.DeserializeObject<Location>(contents);
				if (location == null)
				{
					DebugLog.Error($"Could not deserialize Location at path {path}");
					continue;
				}

				_entities.Add(location.Id, location);
			}

			_campaignLoaded = true;
			return true;
		}

		return DoLoad;
	}

	private readonly Dictionary<string, Entity> _entities = [];

	public TEntity? LoadEntity<TEntity>(string id) where TEntity : Entity
	{
		if (!_entities.TryGetValue(id, out var entity) || entity is not TEntity typedEntity)
		{
			DebugLog.Warn($"Cannot find entity of type {typeof(TEntity).Name} with ID {id}");
			return null;
		}

		return typedEntity;
	}

}