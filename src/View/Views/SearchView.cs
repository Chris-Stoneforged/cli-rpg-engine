using Requests;
using View.Menu;
using Spectre.Console;
using Data.Definitions.Entities;
using View.Menu.Displays;

namespace View.Views;

public class SearchView(Location fromLocation) : AView
{
	private readonly Location _fromLocation = fromLocation;

	public override void CleanUp() { }

	public override async Task Loop()
	{
		var options = _fromLocation.ItemPickups.Select(
			p => new MenuOption(
				new ItemPickupDisplay(p),
				() => OnItemPickedUp(p)))
			.ToArray();

		new MenuBuilder()
			.Title("You find these items")
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnItemPickedUp(ItemPickup pickup)
	{
		Ctx.RequestDispatcher.MakeRequest(new PickUpItemRequest(pickup));
	}
}