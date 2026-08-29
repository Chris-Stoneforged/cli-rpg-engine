using Data.Definitions;

namespace Models.Definitions;

public interface ILocationModel : IModel
{
	Location? CurrentLocation { get; }
}