using Data.Definitions.Entities;
using Debug;

namespace Data;

public class SaveManager
{
	public readonly List<string> SavePaths = [];

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
		DebugLog.Info(saveFolder);
		SavePaths = [.. Directory.EnumerateFiles(saveFolder).Where(p => p.EndsWith(".db"))];
		foreach (var v in SavePaths)
		{
			DebugLog.Info(v);
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

			SavePaths.Add(savePath);
			db.SaveChanges();
		}

		return savePath;
	}
}