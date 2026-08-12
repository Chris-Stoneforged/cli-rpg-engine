using Spectre.Console;
using View.Definitions;

namespace View;

public class LoadContext(StatusContext ctx) : ILoadContext
{
	private readonly StatusContext _ctx = ctx;

	public void SetLoadText(string text)
	{
		_ctx.Status(text);
	}
}