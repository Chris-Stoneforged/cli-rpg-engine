using Debug;
using Models.Definitions;
using Spectre.Console;
using Spectre.Console.Rendering;
using View.Menu;

namespace View.Views;

public class WorldView : AView
{
	private ILocationModel? _locationModel;

	public override void Initialize(ViewContext ctx)
	{
		base.Initialize(ctx);
		_locationModel = ctx.ModelGetter.GetAndNotify<ILocationModel>(OnLocationModelUpdated);
	}

	public void OnLocationModelUpdated(ILocationModel locationModel)
	{
		_locationModel = locationModel;
	}

	public override IRenderable? Before()
	{
		var location = _locationModel?.CurrentLocation;
		if (location == null)
		{
			DebugLog.Error("LocationView - Current location is null");
			return null;
		}

		return new Rule($"[blue]{location.Name}[/]")
		{
			Justification = Justify.Center
		};

	}

	public override async Task Loop()
	{
		//await ViewUtils.RenderTextAsync("A desolate inn at the end of the world. It seems deserted, but you feels somethig in the air");
		AnsiConsole.WriteLine();
		new MenuBuilder()
			.Title("What do you want to do?")
			.AddOption("Navigate", OnNavigateSelected)
			.AddOption("Search", OnSearchSelected)
			.AddOption("Inventory", OnInventorySelected)
			.AddOption("Options", OnOptionsSelected)
			.Execute(Ctx);
	}

	public void OnNavigateSelected()
	{
		if (_locationModel != null && _locationModel.CurrentLocation != null)
		{
			Ctx.ViewManager.ShowView(new NavigateView(_locationModel.CurrentLocation));
		}
	}

	private void OnSearchSelected()
	{
		if (_locationModel != null && _locationModel.CurrentLocation != null)
		{
			Ctx.ViewManager.ShowView(new SearchView(_locationModel.CurrentLocation));
		}
	}

	private void OnInventorySelected()
	{
		Ctx.ViewManager.ShowView(new InventoryView());
	}

	private void OnOptionsSelected()
	{
		Ctx.ViewManager.ShowView(new OptionsView());
	}

	public override void CleanUp()
	{
		Ctx.ModelGetter.UnNotify<ILocationModel>(OnLocationModelUpdated);
	}
}