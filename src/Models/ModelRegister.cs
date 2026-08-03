using Models.Definitions;

namespace Models;

public class ModelRegister : IModelGetter
{
	private readonly List<IModel> _models = [];
	private readonly Dictionary<Type, List<object>> _callbacks = [];

	public ModelRegister RegisterModel<TModel>() where TModel : class, IModel, new()
	{
		if (GetModel<TModel>() != null)
		{
			return this;
		}

		_models.Add(new TModel());

		var type = typeof(TModel);
		if (!_callbacks.ContainsKey(type))
		{
			_callbacks.Add(type, []);
		}

		return this;
	}

	public TModel? GetModel<TModel>() where TModel : class, IModel
	{
		return _models.FirstOrDefault(t => t is TModel) as TModel;
	}

	public void Notify<TModel>(Action<TModel> callback) where TModel : class, IModel
	{
		if (_callbacks.TryGetValue(typeof(TModel), out var list))
		{
			list.Add(callback);
		}
	}

	public TModel? GetAndNotify<TModel>(Action<TModel> callback) where TModel : class, IModel
	{
		Notify(callback);
		return GetModel<TModel>();
	}

	public void UpdateModel<TModel>(Action<TModel> updateMethod) where TModel : class, IModel
	{
		if (_models.FirstOrDefault(m => m is TModel) is not TModel model) return;
		updateMethod(model);
		SendUpdatedNotification(model);
		//Save(_currentSavePath);
	}

	private void SendUpdatedNotification<TModel>(TModel updatedModel) where TModel : class, IModel
	{
		if (!_callbacks.TryGetValue(typeof(TModel), out var callbacks)) return;

		foreach (var callback in callbacks.Cast<Action<TModel>>())
		{
			callback.Invoke(updatedModel);
		}
	}
}