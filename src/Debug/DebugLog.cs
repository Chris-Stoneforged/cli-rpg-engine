namespace Debug;

public static class DebugLog
{
#if DEBUG
	private class ConsoleColorBlock : IDisposable
	{
		private readonly ConsoleColor _previousColor;

		public ConsoleColorBlock(ConsoleColor color)
		{
			_previousColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
		}

		public void Dispose()
		{
			Console.ForegroundColor = _previousColor;
		}
	}

#endif

	public static void Info(string message)
	{
#if DEBUG
		using (new ConsoleColorBlock(ConsoleColor.Green))
		{
			Console.WriteLine($"[{message}]");
		}
#endif
	}

	public static void Warn(string message)
	{
#if DEBUG
		using (new ConsoleColorBlock(ConsoleColor.Yellow))
		{
			Console.WriteLine($"[{message}]");
		}
#endif
	}

	public static void Error(string message)
	{
#if DEBUG
		using (new ConsoleColorBlock(ConsoleColor.Red))
		{
			Console.WriteLine($"[{message}]");
		}
#endif
	}
}