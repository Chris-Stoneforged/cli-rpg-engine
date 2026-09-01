using Requests;
using View.Menu;
using Spectre.Console;
using Data.Definitions.Entities;
using View.Menu.Displays;
using Microsoft.EntityFrameworkCore;

namespace View.Views;

public class SearchView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		var db = Ctx.SessionFactory.GetReadonlySession();

		var core = db.Core
			.Include(c => c.PlayerCharacter)
			.ThenInclude(c => c.Location)
			.ThenInclude(l => l.ItemPickups)
			.ThenInclude(i => i.Item)
			.FirstOrDefault();

		var options = core?.PlayerCharacter?.Location?.ItemPickups
			.Select(p => new MenuOption(
				new ItemPickupDisplay(p),
				() => OnItemPickedUp(p)))
			.ToArray() ?? [];

		new MenuBuilder()
			.Title(options.Length == 0 ? "You find nothing of interest" : "You find these items")
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnItemPickedUp(ItemPickup pickup)
	{
		Ctx.RequestDispatcher.MakeRequest(new PickUpItemRequest(pickup.Id, pickup.Quantity));
	}
}