using Debug;
using Models.Definitions;
using View.Menu;
using View.Menu.Options;

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
			DebugLog.Error("LocationView - Current location is null");
			return;
		}

		await new MenuBuilder()
			.Title(location.Description)
			.AddOption(new BasicMenuOption("Navigate", OnNavigateSelected))
			.AddOption(new BasicMenuOption("Search", OnSearchSelected))
			.AddOption(new BasicMenuOption("Options", OnOptionsSelected))
			.Execute(Ctx);
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