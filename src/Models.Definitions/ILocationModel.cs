using Resources.Definitions.Entities;

namespace Models.Definitions;

public interface ILocationModel : IModel
{
	Location? CurrentLocation { get; }
}