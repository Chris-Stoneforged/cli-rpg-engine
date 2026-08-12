using Models.Definitions;
using Resources.Definitions.Entities;

namespace Models;

public class LocationModel : ILocationModel
{
	public Location? CurrentLocation { get; set; }
}