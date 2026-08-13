using Requests;
using Save.Definitions;

namespace View.Menu.Options;

public class SaveProfileOption(ISaveProfile profile) : AMenuOption
{
	public override string CallToAction { get; } = $"Save {profile.Name} - {profile.LastSavedTime}";
	public override Action Callback
	{
		get
		{
			return MakeLoadGameRequest;
		}
	}


	private void MakeLoadGameRequest()
	{
		Ctx?.RequestDispatcher.MakeRequest(new LoadGameRequest(profile));
	}
}