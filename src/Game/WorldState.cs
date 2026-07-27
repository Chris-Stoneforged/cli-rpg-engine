namespace Game;

public class WorldState
{
	public string CurrentLocationId = "0000";
	private readonly List<string> _flags = [];

	public void SetFlag(string flag, bool value)
	{
		if (value && !_flags.Contains(flag))
		{
			_flags.Add(flag);
		}
		else if (!value && _flags.Contains(flag))
		{
			_flags.Remove(flag);
		}
	}

	public bool IsFlagSet(string flag)
	{
		return _flags.Contains(flag);
	}
}