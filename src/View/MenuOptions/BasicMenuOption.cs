using View.Definitions;

namespace View.MenuOptions;

public class BasicMenuOption(string callToAction, Action callback) : IMenuOption
{
	public string CallToAction { get; } = callToAction;
	public Action Callback { get; } = callback;
}