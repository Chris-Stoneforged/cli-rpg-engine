using View.Menu;
using Spectre.Console;
using Data.Definitions.Entities;
using View.Menu.Displays;
using Events.Definitions.Game;

namespace View.Views;

public class SearchView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		var db = Ctx.SessionFactory.GetReadonlySession();

		var core = db.Core.FirstOrDefault();
		var options = core?.PlayerCharacter?.Location?.ItemPickups
			.Select(p => new MenuOption(
				new ItemPickupDisplay(p),
				() => OnItemSelected(p)))
			.ToArray() ?? [];

		new MenuBuilder()
			.Title("You find these items")
			.TitleWhenNoOptions("You find nothing of interest")
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnItemSelected(ItemPickup pickup)
	{
		new MenuBuilder()
			.Title(pickup.Item?.Name ?? "???")
			.HasBackOption(false)
			.AddOption("Take All", () => OnTakeAllSelected(pickup))
			.AddOption("Take Some", () => OnTakeSomeSelected(pickup))
			.Execute(Ctx);
	}

	private void OnTakeAllSelected(ItemPickup pickup)
	{
		Ctx.EventEmitter.Emit(new PickUpItemEvent(pickup.Id, pickup.Quantity));
	}

	private void OnTakeSomeSelected(ItemPickup pickup)
	{

	}
}