using Spectre.Console;
using View.Definitions;
using View.Menu.Options;

namespace View.Menu;

public class MenuBuilder
{
	private readonly SelectionPrompt<IMenuOption> _prompt =
		new SelectionPrompt<IMenuOption>()
			.UseConverter(o => o.CallToAction)
			.WrapAround();
	private readonly List<IMenuOption> _menuOptions = [];

	private string _title = "";
	private bool _hasBackOption = false;

	public MenuBuilder Title(string title)
	{
		_title = title;
		return this;
	}

	public MenuBuilder AddOption(IMenuOption option)
	{
		_menuOptions.Add(option);
		return this;
	}

	public MenuBuilder AddOptionIf(Func<bool> condition, IMenuOption option)
	{
		if (condition())
		{
			_menuOptions.Add(option);
		}
		return this;
	}

	public MenuBuilder AddOptions(params IMenuOption[] options)
	{
		_menuOptions.AddRange(options);
		return this;
	}

	public MenuBuilder AddOptionsIf(Func<bool> condition, params IMenuOption[] options)
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
			AddOption(new BackOption());
		}

		foreach (var option in _menuOptions)
		{
			if (option is AMenuOption aOption)
			{
				aOption.Initialize(ctx);
			}
		}

		_prompt
			.Title(_title)
			.AddChoices(_menuOptions);

		var choice = AnsiConsole.Prompt(_prompt);
		choice.Callback.Invoke();
	}
}