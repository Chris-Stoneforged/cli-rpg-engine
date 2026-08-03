using UserInterface.Definitions;

namespace Menu;

public class Menu
{
	private readonly List<IMenuOption> _options = [];

	public void AddOption(IMenuOption option)
	{
		_options.Add(option);
	}

	public void Execute(IUserInterface ui)
	{
		var textOptions = _options.Select(o => o.CallToAction).ToList();
		var index = ui.PushUserChoice("What would you like to do?", textOptions);
		_options[index].Callback.Invoke();
	}
}