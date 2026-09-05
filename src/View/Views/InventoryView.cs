using View.Menu;
using Spectre.Console;
using View.Menu.Displays;
using Data.Definitions.Entities;

namespace View.Views;

public class InventoryView() : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		using var db = Ctx.SessionFactory.GetReadonlySession();

		var core = db.Core.FirstOrDefault();
		var options = core?.PlayerCharacter?.InventoryEntries
			.Select(p => new MenuOption(
				new InventoryEntryDisplay(p),
				() => OnInventoryItemSelected(p)))
			.ToArray() ?? [];

		new MenuBuilder()
			.Title("Inventory")
			.TitleWhenNoOptions("Inventory (Emtpy)")
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnInventoryItemSelected(InventoryEntry inventoryEntry)
	{
		new MenuBuilder()
			.Title($"[bold]{inventoryEntry.Item.Name}[/] ({inventoryEntry.Quantity})\n\n[italic]{inventoryEntry.Item.Description}[/]")
			.HasBackOption(false)
			.Execute(Ctx);
	}
}