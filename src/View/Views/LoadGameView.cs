using Requests;
using View.Menu;
using View.Menu.Displays;

namespace View.Views;

public class LoadGameView(IReadOnlyCollection<string> savePaths) : AView
{
	private readonly IReadOnlyCollection<string> _savePaths = savePaths;

	public override void CleanUp() { }

	public override async Task Loop()
	{
		using var db = Ctx.SessionFactory.GetReadonlySession();

		var options = _savePaths
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

	private void OnSaveProfileSelected(string path)
	{
		Ctx.RequestDispatcher.MakeRequest(new LoadGameRequest(path));
	}
}