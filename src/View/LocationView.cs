using Menu;
using Models.Definitions;
using Requests.Definitions;
using Requests;
using UserInterface.Definitions;
using View.Definitions;

namespace View;

public class LocationView : IView
{
	private IUserInterface? _ui;
	private IModelGetter? _modelGetter;
	private IRequestMaker? _requestMaker;

	public void CleanUp() { }

	public void Initialize(IUserInterface ui, IModelGetter modelGetter, IRequestMaker requestMaker)
	{
		_ui = ui;
		_modelGetter = modelGetter;
		_requestMaker = requestMaker;
	}

	public void Loop()
	{
		var locationModel = _modelGetter?.GetModel<ILocationModel>();
		if (locationModel == null)
		{
			Console.WriteLine("Location model is null");
			return;
		}
		var location = _modelGetter?.GetModel<ILocationModel>()?.CurrentLocation;
		if (location == null)
		{
			Console.WriteLine("CurrentLocation is null");
			return;
		}

		Console.WriteLine();
		Console.WriteLine(location.Description);

		var menu = new Menu.Menu();
		foreach (var door in location.Doors)
		{
			var request = new OpenDoorRequest(location.Id, door.DestinationId, door.Id);
			menu.AddOption(
				new BasicMenuOption(
					door.CallToAction,
					() => _requestMaker?.MakeRequest(request)
				)
			);
		}

		if (_ui != null)
		{
			menu.Execute(_ui);
		}
	}
}