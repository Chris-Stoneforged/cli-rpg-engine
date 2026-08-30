using Save.Definitions;
using View.Definitions;

namespace View.Menu.Displays;

public class SaveProfileDisplay(ISaveProfile profile) : IMenuOptionDisplay
{
	public string DisplayMarkup => $"Save {profile.Name} - {profile.LastSavedTime}";
}