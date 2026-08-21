using Requests;
using View.Menu;
using View.Menu.Options;

namespace View;

public class MainMenuView : AView
{
	public override async Task Loop()
	{
		new MenuBuilder()
			.Title("Welcome to the game")
			.AddOption(new BasicMenuOption("New Game", OnNewGameSelected))
			.AddOptionIf(
					() => Ctx.SaveManager.Profiles.Count > 0,
					new BasicMenuOption("Load Game", OnLoadGameSelected)
					)
			.AddOption(new BasicMenuOption("Credits", OnCreditsSelected))
			.AddOption(new BasicMenuOption("Quit", OnQuitSelected))
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

	private void OnCreditsSelected()
	{
		Ctx.ViewManager.ShowView(new CreditsView());
	}

	private void OnQuitSelected()
	{
		Ctx.RequestDispatcher.MakeRequest(new QuitGameRequest());
	}
}