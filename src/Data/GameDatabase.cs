using Microsoft.EntityFrameworkCore;
using Data.Definitions;
using Data.Definitions.Entities;
using Data.Definitions.Encounter;
using Core.Definitions.Enums;
using Data.Definitions.Triggers;

namespace Data;

public class GameDatabase(string campaignPath) : DbContext, IGameDatabase
{
	public DbSet<GameCore> Core { get; set; }
	public DbSet<Location> Locations { get; set; }
	public DbSet<Door> Doors { get; set; }
	public DbSet<Item> Items { get; set; }
	public DbSet<ItemPickup> ItemPickups { get; set; }
	public DbSet<InventoryEntry> InventoryEntries { get; set; }
	public DbSet<Character> Characters { get; set; }
	public DbSet<Encounter> Encounters { get; set; }
	public DbSet<EncounterStepData> EncounterSteps { get; set; }
	public DbSet<DialogueStepData> DialogueSteps { get; set; }
	public DbSet<Trigger> Triggers { get; set; }
	public DbSet<CharacterInteractionTrigger> CharacterEncounterTriggers { get; set; }

	private readonly string _campaignPath = campaignPath;

	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options
		.UseLazyLoadingProxies()
		.UseSqlite($"Data Source={_campaignPath}").EnableSensitiveDataLogging();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Door>()
			.HasOne(e => e.From)
			.WithMany(e => e.DoorsOut)
			.HasForeignKey(e => e.FromId);
		modelBuilder.Entity<Door>()
			.HasOne(e => e.To)
			.WithMany(e => e.DoorsIn)
			.HasForeignKey(e => e.ToId);

		modelBuilder.Entity<ItemPickup>()
			.HasOne(e => e.Location)
			.WithMany(e => e.ItemPickups)
			.HasForeignKey(e => e.LocationId);
		modelBuilder.Entity<ItemPickup>()
			.HasOne(e => e.Item)
			.WithMany(e => e.ItemPickups)
			.HasForeignKey(e => e.ItemId);

		modelBuilder.Entity<InventoryEntry>()
			.HasOne(e => e.Item)
			.WithMany(e => e.InventoryEntries)
			.HasForeignKey(e => e.ItemId);
		modelBuilder.Entity<InventoryEntry>()
			.HasOne(e => e.Owner)
			.WithMany(e => e.InventoryEntries)
			.HasForeignKey(e => e.OwnerId);

		modelBuilder.Entity<Character>()
			.HasOne(e => e.Location)
			.WithMany(e => e.Characters)
			.HasForeignKey(e => e.LocationId);

		modelBuilder.Entity<EncounterStepData>()
			.HasOne(e => e.Encounter)
			.WithMany(e => e.Steps)
			.HasForeignKey(e => e.EncounterId);
		modelBuilder.Entity<EncounterStepData>()
			.HasKey(nameof(EncounterStepData.Id), nameof(EncounterStepData.EncounterId));
		modelBuilder.Entity<EncounterStepData>()
			.HasDiscriminator(e => e.Type)
			.HasValue<EncounterStepData>(EncounterStepType.NONE)
			.HasValue<DialogueStepData>(EncounterStepType.DIALOGUE);

		modelBuilder.Entity<Trigger>()
			.HasDiscriminator(e => e.Type)
			.HasValue<Trigger>(TriggerType.NONE)
			.HasValue<CharacterInteractionTrigger>(TriggerType.CHARACTER_INTERACTION);
	}
}