using View.Definitions;

namespace View.Menu.Options;

public abstract class AMenuOption : IMenuOption
{
	protected ViewContext Ctx => _ctx ?? throw new Exception("MenuOption was not initialized");

	public abstract string CallToAction { get; }
	public abstract Action Callback { get; }

#pragma warning disable IDE0032 // Use auto property
	private ViewContext? _ctx;
#pragma warning restore IDE0032 // Use auto property

	public void Initialize(ViewContext ctx)
	{
		_ctx = ctx;
	}
}