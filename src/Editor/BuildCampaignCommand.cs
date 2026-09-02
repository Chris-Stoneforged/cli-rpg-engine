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
		var campaignPath = result.GetValue(_campaignPathOption)?.ToString();
		var outputPath = result.GetValue(_outputFileOption);

		if (campaignPath == null || outputPath == null)
		{
			return 1;
		}

		var code = await AnsiConsole.Status()
			.Spinner(Spinner.Known.Dots)
			.SpinnerStyle(Style.Parse("green"))
			.StartAsync("Building campaign", async ctx =>
			{
				// Create and clear database
				var db = new GameDatabase(outputPath.ToString());
				db.ChangeTracker.Entries().ToList().ForEach(e => e.State = EntityState.Detached);
				await db.Database.EnsureDeletedAsync(token);
				await db.Database.EnsureCreatedAsync(token);

				await LoadEntities(db.Doors, Path.Combine(campaignPath, "doors"));
				await LoadEntities(db.Locations, Path.Combine(campaignPath, "locations"));
				await LoadEntities(db.Items, Path.Combine(campaignPath, "items"));
				await LoadEntities(db.ItemPickups, Path.Combine(campaignPath, "item-pickups"));
				await LoadEntities(db.Characters, Path.Combine(campaignPath, "characters"));

				await db.SaveChangesAsync();
				return 0;
			});

		if (code == 0)
		{
			AnsiConsole.WriteLine($"Successfully built campaign to {outputPath.FullName}");
		}
		return code;
	}

	public async Task LoadEntities<TEntity>(
		DbSet<TEntity> dbSet,
		string path
	) where TEntity : Entity
	{
		foreach (var p in Directory.EnumerateFiles(path))
		{
			var contents = await File.ReadAllTextAsync(p);
			var entity = JsonConvert.DeserializeObject<TEntity>(contents);
			if (entity == null)
			{
				continue;
			}

			await dbSet.AddAsync(entity);
		}
	}
}