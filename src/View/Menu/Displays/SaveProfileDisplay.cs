using Data.Definitions.Entities;
using View.Definitions;

namespace View.Menu.Displays;

public class SaveProfileDisplay(SaveProfile profile) : IMenuOptionDisplay
{
	public string DisplayMarkup => $"Save {profile.Id}";
}