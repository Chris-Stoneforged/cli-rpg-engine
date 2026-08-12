namespace View.Definitions;

public interface IMenuOption
{
	string CallToAction { get; }
	Action Callback { get; }
}