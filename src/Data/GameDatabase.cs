using Microsoft.EntityFrameworkCore;
using Data.Definitions;
using Data.Definitions.Entities;

namespace Data;

public class GameDatabase(string campaignPath) : DbContext, IGameDatabase
{
	public DbSet<GameCore> Core { get; set; }
	public DbSet<SaveProfile> SaveProfile { get; set; }
	public DbSet<Location> Locations { get; set; }
	public DbSet<Door> Doors { get; set; }
	public DbSet<Item> Items { get; set; }
	public DbSet<ItemPickup> ItemPickups { get; set; }
	public DbSet<InventoryEntry> InventoryEntries { get; set; }

	private readonly string _campaignPath = campaignPath;

	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseSqlite($"Data Source={_campaignPath}").EnableSensitiveDataLogging();

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
	}
}