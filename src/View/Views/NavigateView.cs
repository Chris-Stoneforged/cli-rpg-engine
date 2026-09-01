using Requests;
using View.Menu;
using Spectre.Console;
using Data.Definitions.Entities;
using Microsoft.EntityFrameworkCore;
using Debug;

namespace View.Views;

public class NavigateView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		var db = Ctx.SessionFactory.GetReadonlySession();

		var core = db.Core
			.Include(c => c.CurrentLocation)
			.ThenInclude(l => l.DoorsOut)
			.FirstOrDefault();

		if (core == null)
		{
			DebugLog.Error("Core does not exist");
			return;
		}

		if (core.CurrentLocation == null)
		{
			DebugLog.Error("Current location does not exist");
			return;
		}

		var options = core.CurrentLocation.DoorsOut.Select(
			d => new MenuOption(
				d.CallToAction,
				() => OnLocationSelected(d)
			)
		).ToArray();

		new MenuBuilder()
			.Title("Where do you want to go?")
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnLocationSelected(Door door)
	{
		Ctx.ViewManager.Back();
		Ctx.RequestDispatcher.MakeRequest(new OpenDoorRequest(door.Id));
	}
}