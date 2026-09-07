using Spectre.Console;
using Spectre.Console.Rendering;
using View.Menu;

namespace View.Views;

public class WorldView : AView
{
	public override IRenderable? Before()
	{
		using var db = Ctx.SessionFactory.GetReadonlySession();
		var core = db.Core.FirstOrDefault();

		var locationName = core == null ||
			core.PlayerCharacter == null ||
			core.PlayerCharacter.Location == null ?
				"???" :
				core.PlayerCharacter.Location.Name;

		return new Rule($"[blue]{locationName}[/]")
		{
			Justification = Justify.Center
		};
	}

	public override async Task Loop()
	{
		new MenuBuilder()
			.Title("What do you want to do?")
			.AddOption("Navigate", OnNavigateSelected)
			.AddOption("Search", OnSearchSelected)
			.AddOption("Chat", OnChatSelected)
			.AddOption("Inventory", OnInventorySelected)
			.AddOption("Options", OnOptionsSelected)
			.Execute(Ctx);
	}

	public void OnNavigateSelected()
	{
		Ctx.ViewManager.ShowView(new NavigateView());
	}

	private void OnSearchSelected()
	{
		Ctx.ViewManager.ShowView(new SearchView());
	}

	private void OnChatSelected()
	{
		Ctx.ViewManager.ShowView(new ChatView());
	}

	private void OnInventorySelected()
	{
		Ctx.ViewManager.ShowView(new InventoryView());
	}

	private void OnOptionsSelected()
	{
		Ctx.ViewManager.ShowView(new OptionsView());
	}
}