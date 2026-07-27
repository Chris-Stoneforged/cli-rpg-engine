namespace Game;

class Program
{
	static int Main(string[] args)
	{
		string campaignPath = args.Length >= 1 ? args[0] : "";
		if (!GameInstance.TryLoad(campaignPath, out var gameInstance))
		{
			return 1;
		}

		gameInstance.Run();
		return 0;
	}
}