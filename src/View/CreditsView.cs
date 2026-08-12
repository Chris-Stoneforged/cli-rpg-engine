using Spectre.Console;
using View.Definitions;
using View.MenuOptions;

namespace View;

public class CreditsView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		var backOption = new BackOption();
		backOption.Initialize(Ctx);

		var prompt = new SelectionPrompt<IMenuOption>()
			.Title("Created by Stoneforged Games")
			.UseConverter(m => m.CallToAction)
			.AddChoices(backOption);

		var chosenOption = AnsiConsole.Prompt(prompt);
		chosenOption.Callback.Invoke();
	}
}