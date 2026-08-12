using Debug;
using Models.Definitions;
using Requests.Definitions;
using Spectre.Console;
using View.Definitions;

namespace View;

public class ViewManager() : IViewManager
{
	private readonly Stack<IView> _viewStack = new();
	private ViewContext? _viewContext = null;

	public void Initialize(IRequestDispatcher requestDispatcher, IModelGetter modelGetter)
	{
		_viewContext = new(this, requestDispatcher, modelGetter);
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
}