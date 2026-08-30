using View.Menu;
using Spectre.Console;
using Models.Definitions;
using View.Menu.Displays;
using Debug;

namespace View.Views;

public class InventoryView() : AView
{
	private IInventoryModel? Inventory { get; set; }

	public override void Initialize(ViewContext ctx)
	{
		base.Initialize(ctx);
		Inventory = ctx.ModelGetter.GetAndNotify<IInventoryModel>(UpdateInventoryModel);
	}

	private void UpdateInventoryModel(IInventoryModel model)
	{
		Inventory = model;
	}

	public override void CleanUp()
	{
		Ctx.ModelGetter.UnNotify<IInventoryModel>(UpdateInventoryModel);
	}

	public override async Task Loop()
	{
		var options = Inventory?.InventoryItems
			.Select(i => new MenuOption(
				new InventoryItemDisplay(i),
				() => OnInventoryItemSelected(i)))
			.ToArray() ?? [];

		new MenuBuilder()
			.Title("Inventory")
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnInventoryItemSelected(IInventoryItem inventoryItem)
	{
		DebugLog.Info($"Checking out inventory item {inventoryItem.Item.Name}");
	}
}