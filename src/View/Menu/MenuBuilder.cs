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
	private string _titleIfNoOptions = "";
	private bool _hasBackOption = false;
	private bool _doBack = false;

	public MenuBuilder Title(string title)
	{
		_title = title;
		return this;
	}

	public MenuBuilder TitleWhenNoOptions(string title)
	{
		_titleIfNoOptions = title;
		return this;
	}

	public MenuBuilder AddOption(IMenuOptionDisplay display, Action? callback)
	{
		_menuOptions.Add(new MenuOption(display, callback));
		return this;
	}

	public MenuBuilder AddOptionIf(Func<bool> condition, IMenuOptionDisplay display, Action? callback)
	{
		if (condition())
		{
			_menuOptions.Add(new MenuOption(display, callback));
		}
		return this;
	}

	public MenuBuilder AddOption(string text, Action? callback)
	{
		_menuOptions.Add(new MenuOption(text, callback));
		return this;
	}

	public MenuBuilder AddOptionIf(Func<bool> condition, string text, Action? callback)
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

	public MenuBuilder HasBackOption(bool doBack = true)
	{
		_hasBackOption = true;
		_doBack = doBack;
		return this;
	}

	public void Execute(ViewContext ctx)
	{
		var title = _menuOptions.Count == 0 && !string.IsNullOrEmpty(_titleIfNoOptions) ?
			_titleIfNoOptions :
			_title;

		if (_hasBackOption)
		{
			AddOption(new BackDisplay(), _doBack ? ctx.ViewManager.Back : null);
		}

		_prompt
			.Title($"\n{title}")
			.AddChoices(_menuOptions);

		var choice = AnsiConsole.Prompt(_prompt);
		choice.Callback?.Invoke();
	}
}