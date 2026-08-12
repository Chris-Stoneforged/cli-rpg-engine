namespace Save.Definitions;

public interface ISaveProfile
{
	string Path { get; }
	string Name { get; }
	DateTime LastSavedTime { get; }
}