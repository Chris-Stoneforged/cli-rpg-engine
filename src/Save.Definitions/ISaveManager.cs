namespace Save.Definitions;

public interface ISaveManager
{
	IReadOnlyList<ISaveProfile> Profiles { get; }

	void RegisterSaveHandler<TSaveData>(
		string saveKey,
		SaveCallback<TSaveData> callback
	) where TSaveData : ISaveData;

	void RegisterLoadHandler<TSaveData>(
		string saveKey,
		Action<TSaveData> callback
	) where TSaveData : ISaveData;
}