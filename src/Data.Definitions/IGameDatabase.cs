using Data.Definitions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Definitions;

public interface IGameDatabase : IDisposable
{
	public DbSet<GameCore> Core { get; set; }
	public DbSet<SaveProfile> SaveProfile { get; set; }
	public DbSet<Location> Locations { get; }
	public DbSet<Door> Doors { get; }
	public DbSet<Item> Items { get; }
	public DbSet<ItemPickup> ItemPickups { get; }
	public DbSet<InventoryEntry> InventoryEntries { get; }
}