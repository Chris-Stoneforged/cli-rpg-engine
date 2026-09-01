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

		var header = "Inventory";
		MenuOption[] options = [];

		if (db.InventoryEntries.Count() == 0)
		{
			header += " (Empty)";
		}
		else
		{
			options = db.InventoryEntries
				.Include(e => e.Item)
				.AsEnumerable()
				.Select(p => new MenuOption(
					new InventoryEntryDisplay(p),
					() => OnInventoryItemSelected(p)))
				.ToArray();
		}

		new MenuBuilder()
			.Title(header)
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnInventoryItemSelected(InventoryEntry inventoryEntry)
	{
		DebugLog.Info($"Checking out inventory item {inventoryEntry.Item?.Name}");
	}
}