using Microsoft.EntityFrameworkCore;
using Data.Definitions;

namespace Data;

public class CampaignContext(string campaignPath) : DbContext, ICampaignData
{
	public DbSet<Location> Locations { get; set; }
	public DbSet<Door> Doors { get; set; }

	private readonly string _campaignPath = campaignPath;

	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseSqlite($"Data Source={_campaignPath}");
}