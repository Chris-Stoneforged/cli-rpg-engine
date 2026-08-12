namespace Models.Definitions;

public interface IModelUpdater
{
	public void UpdateModel<TModel>(Action<TModel> updateMethod) where TModel : class, IModel;
}