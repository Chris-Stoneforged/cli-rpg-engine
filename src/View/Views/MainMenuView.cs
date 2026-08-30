using Requests;
using View.Menu;

namespace View.Views;

public class MainMenuView : AView
{
	public override async Task Loop()
	{
		new MenuBuilder()
			.Title("Welcome to the game")
			.AddOption("New Game", OnNewGameSelected)
			.AddOptionIf(
				() => Ctx.SaveManager.Profiles.Count > 0,
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
		Ctx.ViewManager.ShowView(new LoadGameView());
	}

	private void OnQuitSelected()
	{
		Ctx.RequestDispatcher.MakeRequest(new QuitGameRequest());
	}
}