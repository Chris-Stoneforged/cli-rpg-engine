using Models.Definitions;
using Spectre.Console;
using View.Definitions;
using View.MenuOptions;

namespace View;

public class LocationView : AView
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

	public override async Task Loop()
	{
		var location = _locationModel?.CurrentLocation;
		if (location == null)
		{
			Console.WriteLine("CurrentLocation is null");
			Console.ReadLine();
			return;
		}

		var prompt = new SelectionPrompt<IMenuOption>()
			.Title(location.Description)
			.UseConverter(m => m.CallToAction)
			.WrapAround()
			.AddChoices(
				new BasicMenuOption("Navigate", OnNavigateSelected),
				new BasicMenuOption("Search", OnSearchSelected),
				new BasicMenuOption("Options", OnOptionsSelected)
			);

		var chosenOption = AnsiConsole.Prompt(prompt);
		chosenOption.Callback.Invoke();
	}

	public void OnNavigateSelected()
	{
		if (_locationModel != null && _locationModel.CurrentLocation != null)
		{
			Ctx.ViewManager.ShowView(new NavigateView(_locationModel.CurrentLocation));
		}
	}

	private static void OnSearchSelected()
	{

	}

	private void OnOptionsSelected()
	{
		Ctx.ViewManager.ShowView(new OptionsView());
	}

	public override void CleanUp()
	{
		// TODO: Remove listener
	}
}