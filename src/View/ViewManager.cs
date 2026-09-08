using Data.Definitions;
using Debug;
using Events.Definitions;
using Spectre.Console;
using Spectre.Console.Rendering;
using View.Definitions;

namespace View;

public class ViewManager(
	IEventManager eventManager,
	ISessionFactory sessionFactory
) : IViewManager
{
	private readonly Stack<IView> _viewStack = new();
	private readonly Dictionary<ViewKey, IView> _cachedViews = [];
	private readonly IEventManager _eventManager = eventManager;
	private readonly ISessionFactory _sessionFactory = sessionFactory;
	private readonly GameLog.GameLog _gameLogs = new(eventManager);

	private ViewContext? _viewContext = null;

	public void ShowView(IView view)
	{
		_viewContext ??= new ViewContext(this, _eventManager, _sessionFactory);
		if (view is AView aView)
		{
			aView.Initialize(_viewContext);
		}

		_viewStack.Push(view);
	}

	public void ShowCachedView(ViewKey viewKey)
	{
		if (!_cachedViews.TryGetValue(viewKey, out var view))
		{
			DebugLog.Error($"No view cached for key {viewKey}");
			return;
		}

		ShowView(view);
	}

	public void CacheView(ViewKey viewKey, IView view)
	{
		if (!_cachedViews.TryAdd(viewKey, view))
		{
			DebugLog.Error($"Could not cache view with key {viewKey}");
		}
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
		//AnsiConsole.Clear();
		//foreach (var view in _viewStack.Reverse())
		//{
		//	var renderable = view.Before();
		//	if (renderable != null)
		//	{
		//		AnsiConsole.Write(renderable);
		//	}
		//}

		if (_viewStack.TryPeek(out var currentView))
		{
			await currentView.Loop();
		}

		_gameLogs.FlushPending();
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