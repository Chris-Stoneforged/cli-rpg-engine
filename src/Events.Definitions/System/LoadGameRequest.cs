namespace Events.Definitions.System;

public class LoadSaveProfileEvent(string path) : AEvent
{
	public string SavePath { get; } = path;
}