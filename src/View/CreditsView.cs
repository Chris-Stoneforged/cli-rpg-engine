using Menu;
using Models.Definitions;
using Requests;
using Requests.Definitions;
using UserInterface.Definitions;
using View.Definitions;

namespace View;

public class CreditsView : IView
{
	private IUserInterface? _ui;
	private IRequestMaker? _requestMaker;

	public void CleanUp() { }

	public void Initialize(IUserInterface ui, IModelGetter modelGetter, IRequestMaker requestMaker)
	{
		_ui = ui;
		_requestMaker = requestMaker;
	}

	public void Loop()
	{
		Console.WriteLine();
		Console.WriteLine("Created by Stoneforged Games");
		Console.WriteLine();
		Console.WriteLine("  Lead Programmer - Chris Stone");
		Console.WriteLine();

		var menu = new Menu.Menu();
		menu.AddOption(new BasicMenuOption("Back", OnBackSelected));

		if (_ui != null)
		{
			menu.Execute(_ui);
		}
	}

	private void OnBackSelected()
	{
		_requestMaker?.MakeRequest(new PopViewRequest(this));
	}
}