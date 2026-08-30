using Spectre.Console;
using View.Definitions;
using View.Menu.Displays;

namespace View.Menu;

public class MenuBuilder
{
	private readonly SelectionPrompt<MenuOption> _prompt =
		new SelectionPrompt<MenuOption>()
			.UseConverter(o => o.Display.DisplayMarkup)
			.WrapAround();

	private readonly List<MenuOption> _menuOptions = [];

	private string _title = "";
	private bool _hasBackOption = false;

	public MenuBuilder Title(string title)
	{
		_title = title;
		return this;
	}

	public MenuBuilder AddOption(IMenuOptionDisplay display, Action callback)
	{
		_menuOptions.Add(new MenuOption(display, callback));
		return this;
	}

	public MenuBuilder AddOptionIf(Func<bool> condition, IMenuOptionDisplay display, Action callback)
	{
		if (condition())
		{
			_menuOptions.Add(new MenuOption(display, callback));
		}
		return this;
	}

	public MenuBuilder AddOption(string text, Action callback)
	{
		_menuOptions.Add(new MenuOption(text, callback));
		return this;
	}

	public MenuBuilder AddOptionIf(Func<bool> condition, string text, Action callback)
	{
		if (condition())
		{
			_menuOptions.Add(new MenuOption(text, callback));
		}
		return this;
	}

	public MenuBuilder AddOptions(params MenuOption[] options)
	{
		_menuOptions.AddRange(options);
		return this;
	}

	public MenuBuilder AddOptionsIf(Func<bool> condition, params MenuOption[] options)
	{
		if (condition())
		{
			_menuOptions.AddRange(options);
		}
		return this;
	}

	public MenuBuilder HasBackOption()
	{
		_hasBackOption = true;
		return this;
	}

	public void Execute(ViewContext ctx)
	{
		if (_hasBackOption)
		{
			AddOption(new BackDisplay(), ctx.ViewManager.Back);
		}

		_prompt
			.Title(_title)
			.AddChoices(_menuOptions);

		var choice = AnsiConsole.Prompt(_prompt);
		choice.Callback.Invoke();
	}
}