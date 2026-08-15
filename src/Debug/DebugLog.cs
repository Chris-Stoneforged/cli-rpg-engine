namespace Debug;

public static class DebugLog
{
	public static void Info(string message)
	{
#if DEBUG
		DebugHistory.RecordMessage(new DebugRecord(Severity.INFO, message));
#endif
	}

	public static void Warn(string message)
	{
#if DEBUG
		DebugHistory.RecordMessage(new DebugRecord(Severity.WARN, message));
#endif
	}

	public static void Error(string message)
	{
#if DEBUG
		DebugHistory.RecordMessage(new DebugRecord(Severity.ERROR, message));
#endif
	}
}