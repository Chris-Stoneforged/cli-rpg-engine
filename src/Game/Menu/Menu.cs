namespace Game.Menu;

public class Menu
{
	public class MenuOption(string callToAction, Action callback)
	{
		public string CallToAction { get; } = callToAction;
		public Action Callback { get; } = callback;
	}

	private readonly List<MenuOption> _options = [];

	public void AddOption(string callToAction, Action callback)
	{
		_options.Add(new MenuOption(callToAction, callback));
	}

	public void Execute()
	{
		var textOptions = _options.Select(o => o.CallToAction).ToList();
		var index = GameContext.UIController.PushUserChoice("What would you like to do?", textOptions);
		_options[index].Callback.Invoke();
	}
}