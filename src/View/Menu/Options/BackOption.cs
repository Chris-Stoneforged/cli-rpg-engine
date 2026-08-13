namespace View.Menu.Options;

public class BackOption : AMenuOption
{
	public override string CallToAction => "Back";

	public override Action Callback => GoBack;

	private void GoBack()
	{
		Ctx.ViewManager.Back();
	}
}