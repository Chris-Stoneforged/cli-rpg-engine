using Requests;
using Spectre.Console;
using View.Definitions;
using View.MenuOptions;
using Resources.Definitions.Entities;

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
		);

		var backOption = new BackOption();
		backOption.Initialize(Ctx);

		var prompt = new SelectionPrompt<IMenuOption>()
			.Title("Where do you want to go?")
			.WrapAround()
			.UseConverter(m => m.CallToAction)
			.AddChoices(options)
			.AddChoices(backOption);

		var chosenOption = AnsiConsole.Prompt(prompt);
		chosenOption.Callback.Invoke();
	}

	private void OnLocationSelected(string destinationId, string doorId)
	{
		Ctx.ViewManager.Back();
		Ctx.RequestDispatcher.MakeRequest(
			new OpenDoorRequest(_fromLocation.Id, destinationId, doorId)
		);
	}
}