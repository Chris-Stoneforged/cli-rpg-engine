using Menu;
using Models.Definitions;
using Requests.Definitions;
using Requests;
using UserInterface.Definitions;
using View.Definitions;

namespace View;

public class MainMenuView : IView
{
	private IUserInterface? _ui;
	private IRequestMaker? _requestMaker;

	public void CleanUp() { }

	public void Initialize(IUserInterface ui, IModelGetter _, IRequestMaker requestMaker)
	{
		_ui = ui;
		_requestMaker = requestMaker;
	}

	public void Loop()
	{
		Console.WriteLine("TALES OF STONE");
		Console.WriteLine();

		var menu = new Menu.Menu();
		menu.AddOption(new BasicMenuOption("New Game", OnNewGameSelected));
		menu.AddOption(new BasicMenuOption("Credits", OnCreditsSelected));
		menu.AddOption(new BasicMenuOption("Quit", OnQuitSelected));

		if (_ui != null)
		{
			menu.Execute(_ui);
		}
	}

	private void OnNewGameSelected()
	{

	}

	private void OnCreditsSelected()
	{
		_requestMaker?.MakeRequest(new PushViewRequest(new CreditsView()));
	}

	private void OnQuitSelected()
	{
		_requestMaker?.MakeRequest(new QuitGameRequest());
	}
}