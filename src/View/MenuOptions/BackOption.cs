namespace View.MenuOptions;

public class BackOption : AMenuOption
{
	public override string CallToAction => "Back";

	public override Action Callback => GoBack;

	private void GoBack()
	{
		Ctx.ViewManager.Back();
	}
}