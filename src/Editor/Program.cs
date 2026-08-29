using System.CommandLine;

namespace Editor;

class Program
{
	static async Task<int> Main(string[] args)
	{
		var rootCommand = new RootCommand("Editor tool for creating CLI campaigns")
		{
			Subcommands = {
				new BuildCampaignCommand().GetCommand(),
			}
		};

		return await rootCommand.Parse(args).InvokeAsync();
	}
}