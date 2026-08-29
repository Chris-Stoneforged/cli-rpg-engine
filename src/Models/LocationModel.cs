using Data.Definitions;
using Models.Definitions;

namespace Models;

public class LocationModel : ILocationModel
{
	public Location? CurrentLocation { get; set; }
}