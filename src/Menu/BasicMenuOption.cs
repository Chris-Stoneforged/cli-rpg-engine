namespace Menu;

public class BasicMenuOption(string callToAction, Action callback) : IMenuOption
{
	public string CallToAction => callToAction;
	public Action Callback => callback;
}