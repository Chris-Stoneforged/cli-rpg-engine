using Spectre.Console.Rendering;
using View.Definitions;

namespace View;

public abstract class AView : IView
{
	protected ViewContext Ctx => _ctx ?? throw new Exception("View was not initialized");

#pragma warning disable IDE0032 // Use auto property
	private ViewContext? _ctx;
#pragma warning restore IDE0032 // Use auto property

	public virtual void Initialize(ViewContext ctx)
	{
		_ctx = ctx;
	}

	public virtual IRenderable? Before()
	{
		return null;
	}

	public abstract Task Loop();

	public virtual void CleanUp()
	{

	}
}