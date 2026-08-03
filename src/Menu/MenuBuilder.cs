namespace Menu;

public class MenuBuilder
{
	private readonly Menu _menu = new();

	public MenuBuilder AddBasicMenuOption(string callToAction, Action callback)
	{
		_menu.AddOption(new BasicMenuOption(callToAction, callback));
		return this;
	}

	public MenuBuilder AddMenuOption(IMenuOption option)
	{
		_menu.AddOption(option);
		return this;
	}

	public Menu Build()
	{
		return _menu;
	}
}