using View.Menu;
using Spectre.Console;
using View.Menu.Displays;
using Debug;
using Data.Definitions.Entities;
using Microsoft.EntityFrameworkCore;

namespace View.Views;

public class InventoryView() : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		using var db = Ctx.SessionFactory.GetReadonlySession();

		var core = db.Core
			.Include(c => c.PlayerCharacter)
			.ThenInclude(c => c.InventoryEntries)
			.ThenInclude(e => e.Item)
			.FirstOrDefault();

		var options = core?.PlayerCharacter?.InventoryEntries
			.Select(p => new MenuOption(
				new InventoryEntryDisplay(p),
				() => OnInventoryItemSelected(p)))
			.ToArray() ?? [];

		new MenuBuilder()
			.Title(options.Length == 0 ? "Inventory (Empty)" : "Inventory")
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnInventoryItemSelected(InventoryEntry inventoryEntry)
	{
		DebugLog.Info($"Checking out inventory item {inventoryEntry.Item?.Name}");
	}
}