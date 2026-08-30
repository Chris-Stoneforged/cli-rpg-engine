using View.Definitions;
using View.Menu.Displays;

namespace View.Menu;

public class MenuOption
{
	public readonly IMenuOptionDisplay Display;
	public readonly Action Callback;

	public MenuOption(IMenuOptionDisplay display, Action callback)
	{
		Display = display;
		Callback = callback;
	}

	public MenuOption(string text, Action callback)
	{
		Display = new TextDisplay(text);
		Callback = callback;
	}
}