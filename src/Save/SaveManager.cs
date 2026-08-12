using Debug;
using Newtonsoft.Json;
using Save.Definitions;

namespace Save;

public class SaveManager : ISaveManager
{
	private class SaveHandler(
		string saveKey,
		Type dataType,
		SaveCallback<dynamic> callback
	)
	{
		public string SaveKey { get; } = saveKey;
		public Type DataType { get; } = dataType;
		public SaveCallback<dynamic> Callback { get; } = callback;
	}

	private class LoadHandler(
		string saveKey,
		Type dataType,
		Action<dynamic> callback
	)
	{
		public string SaveKey { get; } = saveKey;
		public Type DataType { get; } = dataType;
		public Action<dynamic> Callback { get; } = callback;
	}

	public IReadOnlyList<ISaveProfile> SaveProfiles => _saveProfiles;

	private const string SAVE_LOCATION = "/Users/chris/Documents/personal-projects/cli-rpg-engine/saves";

	private readonly List<LoadHandler> _loadHandlers = [];
	private readonly List<SaveHandler> _saveHandlers = [];
	private readonly List<SaveProfile> _saveProfiles = [];
	private SaveProfile? _currentSave;

	public SaveManager()
	{
		_saveProfiles.Clear();

		foreach (var filePath in Directory.EnumerateFiles(SAVE_LOCATION))
		{
			var rawProfileData = File.ReadLines(filePath).FirstOrDefault();
			if (rawProfileData == null)
			{
				DebugLog.Error($"Save profile is empty at {filePath}");
				continue;
			}

			var profileData = JsonConvert.DeserializeObject<SaveProfile>(rawProfileData);
			if (profileData == null)
			{
				DebugLog.Error($"Save profile could not be deserialized at {filePath}");
				continue;
			}

			profileData.Path = filePath;
			_saveProfiles.Add(profileData);
		}
	}

	public bool CreateNewSaveFile()
	{
		var saveIndex = _saveProfiles.Count > 0 ?
			_saveProfiles.Max(p => int.TryParse(p.Name, out var i) ? i : 0) :
			0;
		var saveName = (saveIndex + 1).ToString();
		var savePath = Path.Combine(SAVE_LOCATION, saveName);
		//if (File.Exists(savePath))
		//{
		//	DebugLog.Error("Save file already exists");
		//	return false;
		//}

		_currentSave = new SaveProfile()
		{
			Path = savePath,
			LastSavedTime = DateTime.UtcNow,
			Name = saveName
		};
		_saveProfiles.Add(_currentSave);

		var saveData = new Dictionary<string, string>();

		LoadGameData(saveData);
		return SaveCurrentProfile(saveData);
	}


	public bool LoadSaveProfile(ISaveProfile profileInfo)
	{
		var saveProfile = _saveProfiles.FirstOrDefault(p => p.Name == profileInfo.Name);
		if (saveProfile == null)
		{
			DebugLog.Error($"Attempting to load invalid save profile {profileInfo.Name}");
			return false;
		}

		var rawSaveData = File.ReadLines(profileInfo.Path).LastOrDefault();
		if (rawSaveData == null)
		{
			DebugLog.Error($"Save data is empty at {profileInfo.Path}");
			return false;
		}

		var saveData = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawSaveData);
		if (saveData == null)
		{
			DebugLog.Error($"Save data could not be deserialized at {profileInfo.Path}");
			return false;
		}

		LoadGameData(saveData);
		return true;
	}

	private void LoadGameData(Dictionary<string, string> saveData)
	{
		foreach (var loadHandler in _loadHandlers)
		{
			if (!saveData.TryGetValue(loadHandler.SaveKey, out var data))
			{
				var newObject = Activator.CreateInstance(loadHandler.DataType);
				if (newObject == null)
				{
					DebugLog.Error($"Unable to create new object of type {loadHandler.DataType.Name}");
				}
				else
				{
					loadHandler.Callback.Invoke(newObject);
				}

				continue;
			}

			var objectData = JsonConvert.DeserializeObject(data, loadHandler.DataType);
			if (objectData == null)
			{
				DebugLog.Error($"Unable to deserialize data for save key '{loadHandler.SaveKey}'");
				continue;
			}

			loadHandler.Callback.Invoke(objectData);
		}
	}

	public void SaveGameData()
	{
		var saveData = new Dictionary<string, string>();
		foreach (var saveHandler in _saveHandlers)
		{
			var data = saveHandler.Callback.Invoke();
			var typedData = Convert.ChangeType(data, saveHandler.DataType);
			var serializedData = JsonConvert.SerializeObject(typedData);
			saveData.Add(saveHandler.SaveKey, serializedData);
		}

		SaveCurrentProfile(saveData);
	}

	private bool SaveCurrentProfile(Dictionary<string, string> saveData)
	{
		if (_currentSave == null)
		{
			DebugLog.Error("Trying to save current profile but it is null");
			return false;
		}

		var serializedProfile = JsonConvert.SerializeObject(_currentSave);
		var serializedData = JsonConvert.SerializeObject(saveData);

		try
		{
			File.WriteAllLines(_currentSave.Path, [serializedProfile, serializedData]);
		}
		catch (Exception e)
		{
			DebugLog.Error($"Failed to save profile - {e.Message}");
			return false;
		}

		return true;
	}

	public void RegisterSaveHandler<TSaveData>(
		string saveKey,
		SaveCallback<TSaveData> callback
	) where TSaveData : ISaveData
	{
		_saveHandlers.Add(new SaveHandler(saveKey, typeof(TSaveData), () => callback()));
	}

	public void RegisterLoadHandler<TSaveData>(
		string saveKey,
		Action<TSaveData> callback
	) where TSaveData : ISaveData
	{
		_loadHandlers.Add(new LoadHandler(saveKey, typeof(TSaveData), dyn => callback(dyn)));
	}
}