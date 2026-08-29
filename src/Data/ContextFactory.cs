using Data.Definitions;

namespace Data;

public class ContextFactory(string campaignPath) : IDataFactory
{
	private readonly string _campaignPath = campaignPath;

	public ICampaignData GetCampaignData()
	{
		return new CampaignContext(_campaignPath);
	}
}