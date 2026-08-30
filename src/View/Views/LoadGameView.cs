using Requests;
using Save.Definitions;
using View.Menu;
using View.Menu.Displays;

namespace View.Views;

public class LoadGameView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		var options = Ctx.SaveManager.Profiles
			.Select(p => new MenuOption(
				new SaveProfileDisplay(p),
				() => OnSaveProfileSelected(p)))
			.ToArray();

		new MenuBuilder()
			.Title("Select a save profile")
			.AddOptions(options)
			.HasBackOption()
			.Execute(Ctx);
	}

	private void OnSaveProfileSelected(ISaveProfile profile)
	{
		Ctx.RequestDispatcher.MakeRequest(new LoadGameRequest(profile));
	}
}