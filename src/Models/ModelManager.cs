using Debug;
using Models.Definitions;

namespace Models;

public class ModelManager : IModelGetter, IModelUpdater
{
	private readonly List<IModel> _models = [];
	private readonly List<object> _callbacks = [];

	public static ModelManager Create()
	{
		return new ModelManager()
			.RegisterModel<LocationModel>();
	}

	public ModelManager RegisterModel<TModel>() where TModel : class, IModel, new()
	{
		var type = typeof(TModel);
		if (GetModel<TModel>() != null)
		{
			DebugLog.Warn($"Cannot register duplicate model of type {type.Name}");
			return this;
		}

		_models.Add(new TModel());
		return this;
	}

	public TModel? GetModel<TModel>() where TModel : class, IModel
	{
		return _models.FirstOrDefault(t => t is TModel) as TModel;
	}

	public void Notify<TModel>(Action<TModel> callback) where TModel : class, IModel
	{
		_callbacks.Add(callback);
	}

	public TModel? GetAndNotify<TModel>(Action<TModel> callback) where TModel : class, IModel
	{
		Notify(callback);
		return GetModel<TModel>();
	}

	public void UpdateModel<TModel>(Action<TModel> updateMethod) where TModel : class, IModel
	{
		if (_models.FirstOrDefault(m => m is TModel) is not TModel model)
		{
			DebugLog.Warn($"Cannot update model {typeof(TModel).Name} - Model is not registered");
			return;
		}

		updateMethod(model);
		SendUpdatedNotification(model);
	}

	private void SendUpdatedNotification<TModel>(TModel updatedModel) where TModel : class, IModel
	{
		foreach (var callback in _callbacks)
		{
			if (callback is Action<TModel> typedCallback)
			{
				typedCallback.Invoke(updatedModel);
			}
		}
	}

	void IModelGetter.UnNotify<TModel>(Action<TModel> callback)
	{
		_callbacks.Remove(callback);
	}
}