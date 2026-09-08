using Events.Definitions;
using Events.Definitions.Game;
using Spectre.Console;

namespace View.GameLog;

public class GameLog
{
	private const string OddLogColor = "#888888";
	private const string EvenLogColor = "#aaaaaa";

	private readonly Queue<GameLogEntry> _pendingLogs = [];
	private int _numLogs = 0;

	public GameLog(IEventHandler eventHandler)
	{
		eventHandler.Register<PickUpItemEvent>(NotifyPickUpItem, EventPriority.Notification);
	}

	public void FlushPending()
	{
		while (_pendingLogs.TryDequeue(out var log))
		{
			WriteLog(log);
			_numLogs++;
		}
	}

	public void WriteLog(GameLogEntry entry)
	{
		var style = entry.Type switch
		{
			GameLogType.NOTIFICATION => "italic",
			_ => ""
		};
		var color = _numLogs % 2 == 0 ? EvenLogColor : OddLogColor;
		AnsiConsole.MarkupLine($"[{style} {color}]{entry.Message}[/]");
	}

	private void NotifyPickUpItem(PickUpItemEvent @event)
	{
		_pendingLogs.Enqueue(
			new GameLogEntry(
				$"You picked up {@event.ItemName} x{@event.Amount}",
				GameLogType.NOTIFICATION)
			);
	}
}