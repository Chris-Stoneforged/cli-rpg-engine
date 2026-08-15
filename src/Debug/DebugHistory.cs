namespace Debug;

public class DebugHistory
{
	private static DebugHistory Instance
	{
		get
		{
			field ??= new DebugHistory();
			return field;
		}
	} = null;

	private const int LIMIT = 5;

	public static IReadOnlyList<DebugRecord> Messages => [.. Instance._messages];

	private readonly Queue<DebugRecord> _messages = new();

	public static void RecordMessage(DebugRecord record)
	{
		Instance.RecordMessageInternal(record);
	}

	private void RecordMessageInternal(DebugRecord message)
	{
		_messages.Enqueue(message);
		while (_messages.Count > LIMIT)
		{
			_messages.Dequeue();
		}
	}
}