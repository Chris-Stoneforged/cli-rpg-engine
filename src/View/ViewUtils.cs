using Spectre.Console;

namespace View;

public static class ViewUtils
{
	public static async Task RenderTextAsync(string text)
	{
		await AnsiConsole.Live(new Text("")).StartAsync(async ctx =>
		{
			for (var i = 1; i <= text.Length; ++i)
			{
				var delay = text[i - 1] switch
				{
					'.' => 250,
					',' => 150,
					_ => 30
				};

				await Task.Delay(delay);
				ctx.UpdateTarget(new Text(text[..i]));
			}
		});
	}
}