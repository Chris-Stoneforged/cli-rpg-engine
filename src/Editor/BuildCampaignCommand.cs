namespace Editor;

using System.CommandLine;
using Data;
using Data.Definitions.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Spectre.Console;

public class BuildCampaignCommand
{
	private readonly Option<DirectoryInfo> _campaignPathOption;
	private readonly Option<FileInfo> _outputFileOption;

	public BuildCampaignCommand()
	{
		_campaignPathOption = new Option<DirectoryInfo>("--campaign-path", "-p")
		{
			Description = "The path to the campaign JSON files to build",
			DefaultValueFactory = result => new DirectoryInfo(Directory.GetCurrentDirectory()),
		};

		_outputFileOption = new Option<FileInfo>("--output-path", "-o")
		{
			Description = "The output file to create",
			DefaultValueFactory = result => new FileInfo(
				Path.Combine(Directory.GetCurrentDirectory(), "campaign.db")
			)
		};
	}

	public Command GetCommand()
	{
		var command = new Command(
			"build-campaign",
			"Build an sqlite database from your campaign JSON files"
		)
		{
			Options = { _campaignPathOption, _outputFileOption }
		};

		command.SetAction(Build);
		return command;
	}

	public async Task<int> Build(ParseResult result, CancellationToken token)
	{
		var campaignPath = result.GetValue(_campaignPathOption);
		var outputPath = result.GetValue(_outputFileOption);

		if (campaignPath == null || outputPath == null)
		{
			return 1;
		}

		var code = 0;//await AnsiConsole.Status()
					 //.Spinner(Spinner.Known.Dots)
					 //.SpinnerStyle(Style.Parse("green"))
					 //.StartAsync("Building campaign", async ctx =>
					 //{
					 // Create and clear database
		var db = new CampaignContext(outputPath.ToString());
		db.ChangeTracker.Entries().ToList().ForEach(e => e.State = EntityState.Detached);
		await db.Database.EnsureDeletedAsync(token);
		await db.Database.EnsureCreatedAsync(token);

		var doorsPath = Path.Combine(campaignPath.ToString(), "doors");
		foreach (var p in Directory.EnumerateFiles(doorsPath))
		{
			var contents = await File.ReadAllTextAsync(p);
			var door = JsonConvert.DeserializeObject<Door>(contents);
			if (door == null)
			{
				continue;
			}

			await db.Doors.AddAsync(door);
		}

		var locationsPath = Path.Combine(campaignPath.ToString(), "locations");
		foreach (var p in Directory.EnumerateFiles(locationsPath))
		{
			var contents = await File.ReadAllTextAsync(p);
			var location = JsonConvert.DeserializeObject<Location>(contents);
			if (location == null)
			{
				continue;
			}

			await db.Locations.AddAsync(location);
		}

		var itemsPath = Path.Combine(campaignPath.ToString(), "items");
		foreach (var p in Directory.EnumerateFiles(itemsPath))
		{
			var contents = await File.ReadAllTextAsync(p);
			var item = JsonConvert.DeserializeObject<Item>(contents);
			if (item == null)
			{
				continue;
			}

			await db.Items.AddAsync(item);
		}


		var itemPickupsPath = Path.Combine(campaignPath.ToString(), "item-pickups");
		foreach (var p in Directory.EnumerateFiles(itemPickupsPath))
		{
			var contents = await File.ReadAllTextAsync(p);
			var itemPickup = JsonConvert.DeserializeObject<ItemPickup>(contents);
			if (itemPickup == null)
			{
				continue;
			}

			await db.ItemPickups.AddAsync(itemPickup);
		}

		await db.SaveChangesAsync();
		return 0;
		//});

		if (code == 0)
		{
			AnsiConsole.WriteLine($"Successfully built campaign to {outputPath.FullName}");
		}
		return code;
	}
}