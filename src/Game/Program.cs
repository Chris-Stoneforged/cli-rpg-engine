using Spectre.Console;

namespace Game;

class Program
{
	static async Task Main(string[] args)
	{
		AnsiConsole.Clear();
		string campaignPath = args.Length >= 1 ? args[0] : "";
		var gameInstance = new GameInstance(campaignPath);
		await gameInstance.Run();
	}
}