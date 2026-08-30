using Requests;
using View.Menu;
using Spectre.Console;
using Data.Definitions.Entities;

namespace View.Views;

public class NavigateView(Location fromLocation) : AView
{
	private readonly Location _fromLocation = fromLocation;

	public override void CleanUp() { }

	public override async Task Loop()
	{
		var options = _fromLocation.DoorsOut.Select(
			d => new MenuOption(
				d.CallToAction,
				() => OnLocationSelected(d)
			)
		).ToArray();

		new MenuBuilder()
			.Title("Where do you want to go?")
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnLocationSelected(Door door)
	{
		Ctx.ViewManager.Back();
		Ctx.RequestDispatcher.MakeRequest(new OpenDoorRequest(door));
	}
}