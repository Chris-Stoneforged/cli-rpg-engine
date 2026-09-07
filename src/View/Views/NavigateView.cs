using View.Menu;
using Spectre.Console;
using Data.Definitions.Entities;
using Microsoft.EntityFrameworkCore;
using Debug;
using Events.Definitions.Game;

namespace View.Views;

public class NavigateView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		var db = Ctx.SessionFactory.GetReadonlySession();

		var core = db.Core.FirstOrDefault();
		if (core == null)
		{
			DebugLog.Error("Core does not exist");
			return;
		}

		if (core.PlayerCharacter == null)
		{
			DebugLog.Error("Could not get player character");
			return;
		}

		if (core.PlayerCharacter.Location == null)
		{
			DebugLog.Error("Could not get player's location");
			return;
		}

		var options = core.PlayerCharacter.Location.DoorsOut.Select(
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
		Ctx.EventEmitter.Emit(new OpenDoorEvent(door.Id));
	}
}