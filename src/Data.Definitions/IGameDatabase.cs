using Data.Definitions.Entities;
using Data.Definitions.Encounter;
using Microsoft.EntityFrameworkCore;

namespace Data.Definitions;

public interface IGameDatabase : IDisposable
{
	public DbSet<GameCore> Core { get; set; }
	public DbSet<Location> Locations { get; }
	public DbSet<Door> Doors { get; }
	public DbSet<Item> Items { get; }
	public DbSet<ItemPickup> ItemPickups { get; }
	public DbSet<InventoryEntry> InventoryEntries { get; }
	public DbSet<Character> Characters { get; }
	public DbSet<Encounter.Encounter> Encounters { get; }
	public DbSet<EncounterStepData> EncounterSteps { get; }
	public DbSet<DialogueStepData> DialogueSteps { get; }
}