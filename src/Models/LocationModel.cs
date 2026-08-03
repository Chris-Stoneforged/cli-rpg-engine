using Models.Definitions;
using Resources.Definitions;
using Resources.Definitions.Entities;

namespace Models;

public class LocationModel : ILocationModel
{
	public Location? CurrentLocation
	{
		get;
		set
		{
			field = value;
			if (value != null)
			{
				_locationId = value.Id;
			}
		}
	}

	private string _locationId = "0000";

	public void OnLoaded(IEntityLoader entityLoader)
	{
		Console.WriteLine("Loading Location Model");
		CurrentLocation = entityLoader.LoadEntity<Location>(_locationId);
	}
}