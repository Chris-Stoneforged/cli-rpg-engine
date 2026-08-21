using View.Menu;

namespace View;

public class CreditsView : AView
{
	public override void CleanUp() { }

	public override async Task Loop()
	{
		new MenuBuilder()
			.Title("Created by Stoneforged Games")
			.HasBackOption()
			.Execute(Ctx);
	}
}