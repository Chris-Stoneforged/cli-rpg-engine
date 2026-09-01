using Data.Definitions.Entities;
using Debug;

namespace Data;

public class SaveManager
{
	public readonly List<SaveProfile> CachedProfiles = [];

	private readonly string _campaignPath;

	public SaveManager(string campaignPath)
	{
		_campaignPath = campaignPath;

		var dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		var saveFolder = Path.Combine(dataFolder, "Stoneforged");
		if (!Directory.Exists(saveFolder))
		{
			return;
		}

		foreach (var filePath in Directory.EnumerateFiles(saveFolder).Where(p => p.EndsWith(".db")))
		{
			Console.WriteLine(filePath);
			using var db = new GameDatabase(filePath);

			var saveData = db.SaveProfile.FirstOrDefault();
			if (saveData == null)
			{
				DebugLog.Error($"No save profile info for file at {filePath}");
				continue;
			}

			CachedProfiles.Add(
				new SaveProfile()
				{
					Id = saveData.Id,
					FilePath = saveData.FilePath
				}
			);
		}
	}

	public string? CreateSaveProfile()
	{
		var dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		var saveFolder = Path.Combine(dataFolder, "Stoneforged");
		if (!Directory.Exists(saveFolder))
		{
			Directory.CreateDirectory(saveFolder);
		}

		var index = Directory
			.EnumerateFiles(saveFolder)
			.Count(f => f.EndsWith(".db")) + 1;

		var savePath = Path.Combine(saveFolder, $"{index}.db");
		File.Copy(_campaignPath, savePath);

		using (var db = new GameDatabase(savePath))
		{
			var location = db.Locations.FirstOrDefault();
			if (location == null)
			{
				DebugLog.Error("Could not get first location");
				return null;
			}

			db.Core.Add(
				new GameCore()
				{
					PlayerCharacter = new Character()
					{
						Name = "Character",
						Location = location,
						InventoryEntries = []
					}
				}
			);

			var saveProfile = new SaveProfile()
			{
				Id = index,
				FilePath = savePath
			};

			CachedProfiles.Add(saveProfile);
			db.SaveProfile.Add(saveProfile);
			db.SaveChanges();
		}

		return savePath;
	}
}