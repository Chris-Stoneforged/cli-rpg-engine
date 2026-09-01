using Data.Definitions;

namespace Data;

public class SessionFactory(string campaignPath) : ISessionFactory
{
	private string _campaignPath = campaignPath;

	public void SetDatabasePath(string path)
	{
		_campaignPath = path;
	}

	public IGameDatabase GetReadonlySession()
	{
		return GetSession();
	}

	public GameDatabase GetSession()
	{
		return new GameDatabase(_campaignPath);
	}
}