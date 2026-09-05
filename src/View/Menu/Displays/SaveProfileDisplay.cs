using View.Definitions;

namespace View.Menu.Displays;

public class SaveProfileDisplay(string path) : IMenuOptionDisplay
{
	public string DisplayMarkup => $"Save {new FileInfo(path).Name}";
}