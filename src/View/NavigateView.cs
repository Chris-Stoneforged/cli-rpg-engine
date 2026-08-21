using Requests;
using Resources.Definitions.Entities;
using View.Menu.Options;
using View.Menu;
using Spectre.Console;

namespace View;

public class NavigateView(Location fromLocation) : AView
{
	private readonly Location _fromLocation = fromLocation;

	public override void CleanUp() { }

	public override async Task Loop()
	{
		var options = _fromLocation.Doors.Select(
			d => new BasicMenuOption(
				d.CallToAction,
				() => OnLocationSelected(d.DestinationId, d.Id)
			)
		).ToArray();

		new MenuBuilder()
			.Title("Where do you want to go?")
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnLocationSelected(string destinationId, string doorId)
	{
		Ctx.ViewManager.Back();
		Ctx.RequestDispatcher.MakeRequest(
			new OpenDoorRequest(_fromLocation.Id, destinationId, doorId)
		);
	}
}