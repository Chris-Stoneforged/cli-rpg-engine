namespace Models.Definitions;

public interface IModelGetter
{
	void Notify<TModel>(Action<TModel> callback) where TModel : class, IModel;
	TModel? GetModel<TModel>() where TModel : class, IModel;
	TModel? GetAndNotify<TModel>(Action<TModel> callback) where TModel : class, IModel;
}