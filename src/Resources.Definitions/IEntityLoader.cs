using Resources.Definitions.Entities;

namespace Resources.Definitions;

public interface IEntityLoader
{
	TEntity? LoadEntity<TEntity>(string id) where TEntity : Entity;
}