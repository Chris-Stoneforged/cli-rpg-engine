using Requests;
using Spectre.Console;
using View.Definitions;
using View.MenuOptions;

namespace View;

public class OptionsView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		var backOption = new BackOption();
		backOption.Initialize(Ctx);

		var prompt = new SelectionPrompt<IMenuOption>()
			.WrapAround()
			.UseConverter(m => m.CallToAction)
			.AddChoices(
				new BasicMenuOption("Save Game", OnSaveGameSelected),
				new BasicMenuOption("Load Game", OnLoadGameSelected),
				new BasicMenuOption("Main Menu", OnMainMenuSelected),
				backOption
			);

		var chosenOption = AnsiConsole.Prompt(prompt);
		chosenOption.Callback.Invoke();
	}

	private void OnMainMenuSelected()
	{
		Ctx.RequestDispatcher.MakeRequest(new ReturnToMainMenuRequest());
	}

	private void OnSaveGameSelected()
	{
		Ctx.RequestDispatcher.MakeRequest(new SaveGameRequest());
	}

	private void OnLoadGameSelected()
	{
	}
}