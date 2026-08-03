namespace Menu;

public interface IMenuOption
{
	string CallToAction { get; }
	Action Callback { get; }
}