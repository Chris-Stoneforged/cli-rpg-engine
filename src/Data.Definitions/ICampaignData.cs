using Microsoft.EntityFrameworkCore;

namespace Data.Definitions;

public interface ICampaignData : IDisposable
{
	public DbSet<Location> Locations { get; }
	public DbSet<Door> Doors { get; }
}