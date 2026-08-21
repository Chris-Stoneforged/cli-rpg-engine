using Debug;
using Models.Definitions;
using Requests.Definitions;
using Save.Definitions;
using Spectre.Console;
using Spectre.Console.Rendering;
using View.Definitions;

namespace View;

public class ViewManager() : IViewManager
{
	private readonly Stack<IView> _viewStack = new();
	private ViewContext? _viewContext = null;

	public void Initialize(
		IRequestDispatcher requestDispatcher,
		IModelGetter modelGetter,
		ISaveManager saveManager
	)
	{
		_viewContext = new(this, requestDispatcher, modelGetter, saveManager);
	}

	public void ShowView(IView view)
	{
		if (view is AView aView && _viewContext != null)
		{
			aView.Initialize(_viewContext);
		}

		_viewStack.Push(view);
	}

	public void Back()
	{
		if (_viewStack.TryPop(out var currentView))
		{
			currentView.CleanUp();
		}
	}

	public async Task Show()
	{
		AnsiConsole.Clear();
		AnsiConsole.Write(RenderDebugLogs());
		foreach (var view in _viewStack.Reverse())
		{
			var renderable = view.Before();
			if (renderable != null)
			{
				AnsiConsole.Write(renderable);
			}
		}

		if (_viewStack.TryPeek(out var currentView))
		{
			await currentView.Loop();
		}
	}

	public void ResetStack()
	{
		while (_viewStack.TryPop(out var currentView))
		{
			currentView.CleanUp();
		}
	}

	public async Task ShowLoad(Loader loader)
	{
		AnsiConsole.Clear();

		await AnsiConsole.Status()
			.Spinner(Spinner.Known.Arc)
			.SpinnerStyle(Style.Parse("green"))
			.StartAsync("Loading...", async ctx =>
				{
					var loadContext = new LoadContext(ctx);
					await loader(loadContext);
				}
			);
	}

	public async Task<T> ShowLoad<T>(Loader<T> loader)
	{
		AnsiConsole.Clear();

		return await AnsiConsole.Status()
			.Spinner(Spinner.Known.Arc)
			.SpinnerStyle(Style.Parse("green"))
			.StartAsync("Loading...", async ctx =>
				{
					var loadContext = new LoadContext(ctx);
					return await loader(loadContext);
				}
			);
	}

	private Renderable RenderDebugLogs()
	{
#if DEBUG
		var debugGrid = new Grid()
			.AddColumn()
			.AddColumn();
		foreach (var message in DebugHistory.Messages)
		{
			var color = message.Severity switch
			{
				Severity.INFO => "purple",
				Severity.WARN => "yellow",
				Severity.ERROR => "red",
				_ => throw new NotImplementedException()
			};

			debugGrid.AddRow(message.Severity.ToString(), $"[{color}]{message.Message}[/]");
		}

		var debugPanel = new Panel(debugGrid).Header("Debug Logs", Justify.Center).Expand();
		return debugPanel;
#endif
	}
}