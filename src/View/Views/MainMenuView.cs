using Requests;
using View.Definitions;
using View.Menu;

namespace View.Views;

public class MainMenuView(IReadOnlyCollection<string> savePaths) : AView
{
	private readonly IReadOnlyCollection<string> _saveProfiles = savePaths;

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