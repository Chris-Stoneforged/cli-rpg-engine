using Requests;
using Spectre.Console;
using View.Definitions;
using View.MenuOptions;
using Save.Definitions;

namespace View;

public class MainMenuView(IReadOnlyList<ISaveProfile> saveProfiles) : AView
{
	private enum MenuState
	{
		MAIN,
		LOAD
	};

	private MenuState _state = MenuState.MAIN;

	private readonly IReadOnlyList<ISaveProfile> _saveProfiles = saveProfiles;

	public override void CleanUp() { }

	public override async Task Loop()
	{
		switch (_state)
		{
			case MenuState.MAIN:
				RenderMainState();
				break;
			case MenuState.LOAD:
				RenderLoadState();
				break;
		}
	}

	private void RenderMainState()
	{
		var options = new List<IMenuOption>();
		options.Add(new BasicMenuOption("New Game", OnNewGameSelected));
		if (_saveProfiles.Count > 0)
		{
			options.Add(new BasicMenuOption("Load Game", () => _state = MenuState.LOAD));
		}
		options.Add(new BasicMenuOption("Credits", OnCreditsSelected));
		options.Add(new BasicMenuOption("Quit", OnQuitSelected));

		var prompt = new SelectionPrompt<IMenuOption>()
			.Title("Welcome to the game")
			.WrapAround()
			.UseConverter(m => m.CallToAction)
			.AddChoices(options);

		var chosenOption = AnsiConsole.Prompt(prompt);
		chosenOption.Callback.Invoke();
	}

	private void RenderLoadState()
	{
		SaveProfileOption CreateOption(ISaveProfile profile)
		{
			var o = new SaveProfileOption(profile);
			o.Initialize(Ctx);
			return o;
		}

		var choices = _saveProfiles.Select(CreateOption).Cast<IMenuOption>().ToList();
		choices.Add(new BasicMenuOption("Back", () => _state = MenuState.MAIN));

		var prompt = new SelectionPrompt<IMenuOption>()
			.Title("Select a save profile")
			.WrapAround()
			.UseConverter(m => m.CallToAction)
			.AddChoices(choices);

		var chosenOption = AnsiConsole.Prompt(prompt);
		chosenOption.Callback.Invoke();
	}

	private void OnNewGameSelected()
	{
		Ctx.RequestDispatcher.MakeRequest(new NewGameRequest());
	}

	private void OnCreditsSelected()
	{
		Ctx.ViewManager.ShowView(new CreditsView());
	}

	private void OnQuitSelected()
	{
		Ctx.RequestDispatcher.MakeRequest(new QuitGameRequest());
	}
}