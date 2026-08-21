using Requests;
using View.Menu;
using View.Menu.Options;

namespace View;

public class OptionsView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		new MenuBuilder()
			.Title("Options")
			.HasBackOption()
			.AddOption(new BasicMenuOption("Save Game", OnSaveGameSelected))
			.AddOption(new BasicMenuOption("Load Game", OnLoadGameSelected))
			.AddOption(new BasicMenuOption("Main Menu", OnMainMenuSelected))
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
	}
}