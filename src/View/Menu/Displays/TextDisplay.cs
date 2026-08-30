using View.Definitions;

namespace View.Menu.Displays;

public class TextDisplay(string text) : IMenuOptionDisplay
{
	public string DisplayMarkup => text;
}