namespace View.GameLog;

public class GameLogEntry(string message, GameLogType type)
{
	public string Message { get; } = message;
	public GameLogType Type { get; } = type;
	public DateTime Time { get; } = DateTime.Now;
}