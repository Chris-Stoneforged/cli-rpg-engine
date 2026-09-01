using Requests;
using View.Definitions;
using View.Menu;

namespace View.Views;

public class OptionsView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		new MenuBuilder()
			.Title("Options")
			.HasBackOption()
			.AddOption("Save Game", OnSaveGameSelected)
			.AddOption("Load Game", OnLoadGameSelected)
			.AddOption("Main Menu", OnMainMenuSelected)
			.Execute(Ctx);
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
		Ctx.ViewManager.ShowCachedView(ViewKey.LOAD_GAME);
	}
}