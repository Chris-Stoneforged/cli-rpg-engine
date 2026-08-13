using View.Definitions;
using View.Menu;
using View.Menu.Options;

namespace View;

public class LoadGameView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		var options = Ctx.SaveManager.Profiles.Select(p => new SaveProfileOption(p)).Cast<IMenuOption>().ToArray();

		await new MenuBuilder()
			.Title("Select a save profile")
			.AddOptions(options)
			.HasBackOption()
			.Execute(Ctx);
	}
}