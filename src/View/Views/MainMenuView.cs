using Data.Definitions.Entities;
using Requests;
using View.Definitions;
using View.Menu;

namespace View.Views;

public class MainMenuView(IReadOnlyCollection<SaveProfile> saveProfiles) : AView
{
	private readonly IReadOnlyCollection<SaveProfile> _saveProfiles = saveProfiles;

	public override async Task Loop()
	{
		new MenuBuilder()
			.Title("Welcome to the game")
			.AddOption("New Game", OnNewGameSelected)
			.AddOptionIf(
				() => _saveProfiles.Count() > 0,
				"Load Game",
				OnLoadGameSelected
			)
			.AddOption("Quit", OnQuitSelected)
			.Execute(Ctx);
	}

	public override void CleanUp() { }

	private void OnNewGameSelected()
	{
		Ctx.RequestDispatcher.MakeRequest(new NewGameRequest());
	}

	private void OnLoadGameSelected()
	{
		Ctx.ViewManager.ShowCachedView(ViewKey.LOAD_GAME);
	}

	private void OnQuitSelected()
	{
		Ctx.RequestDispatcher.MakeRequest(new QuitGameRequest());
	}
}