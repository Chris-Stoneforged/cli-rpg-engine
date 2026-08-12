namespace Game;

class Program
{
	static async Task<int> Main(string[] args)
	{
		string campaignPath = args.Length >= 1 ? args[0] : "";
		var gameInstance = await GameInstance.Create(campaignPath);
		if (gameInstance == null)
		{
			return 1;
		}

		await gameInstance.Run();
		return 0;
	}
}