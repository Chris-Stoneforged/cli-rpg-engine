using View.Menu;
using Spectre.Console;
using Data.Definitions.Entities;
using Microsoft.EntityFrameworkCore;
using Debug;
using Events.Definitions.Game;

namespace View.Views;

public class ChatView : AView
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

		var options = core.PlayerCharacter.Location.Characters
			.Where(c => c != core.PlayerCharacter)
			.Select(
				c => new MenuOption(
					c.Name,
					() => OnCharacterSelected(c)
			)
		).ToArray();

		new MenuBuilder()
			.Title("Who do you want to talk to?")
			.TitleWhenNoOptions("There appears to be nobody around")
			.HasBackOption()
			.AddOptions(options)
			.Execute(Ctx);
	}

	private void OnCharacterSelected(Character character)
	{
		Ctx.EventEmitter.Emit(new CharacterInteractionEvent(character.Id));
	}
}