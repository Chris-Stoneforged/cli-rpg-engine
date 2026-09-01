using Data.Definitions.Entities;
using Requests;
using View.Menu;
using View.Menu.Displays;

namespace View.Views;

public class LoadGameView(IReadOnlyCollection<SaveProfile> saveProfiles) : AView
{
	private readonly IReadOnlyCollection<SaveProfile> _saveProfiles = saveProfiles;

	public override void CleanUp() { }

	public override async Task Loop()
	{
		using var db = Ctx.SessionFactory.GetReadonlySession();

		var options = _saveProfiles
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

	private void OnSaveProfileSelected(SaveProfile profile)
	{
		Ctx.RequestDispatcher.MakeRequest(new LoadGameRequest(profile.FilePath));
	}
}